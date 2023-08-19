﻿namespace AdventOfCode._2015;
[Challenge(2015, 22)]
internal class Day22_WizardSimulator20XX
{
    private readonly static Spell[] s_spells =
    {
        new("Magic Missile", ManaCost: 53, Cast: (_, t) => t.TakeDamage(4)),
        new("Drain", ManaCost: 73, Cast: (c, t) => { t.TakeDamage(2); c.Heal(2); }),
        new("Shield", ManaCost: 113, Cast: (c, _) => c.GiveEffect(Effect.Shield(6)), CanCast: (c, _) => !c.HasEffect(EffectType.Shield)),
        new("Poison", ManaCost: 173, Cast: (_, t) => t.GiveEffect(Effect.Poison(6)), CanCast: (c, t) => !t.HasEffect(EffectType.Poison)),
        new("Recharge", ManaCost: 229, Cast: (c, _) => c.GiveEffect(Effect.Recharge(5)), CanCast: (c, _) => !c.HasEffect(EffectType.Recharge))
    };

    [Part(1)]
    public void Part01()
    {
        var game = new Game();

        while (true)
        {
            if (game.DoTurn()) break;
        }

        var playerWon = game.PlayerWon();
        Console.WriteLine($"player won: {playerWon}");
    }

    public class Game
    {
        private readonly Player _player = new(50, 500);
        private readonly Boss _boss = new(71, 10);

        private Entity _target;
        private Entity _attacker;

        private bool _gameEnded;

        public Game()
        {
            _target = _boss;
            _attacker = _player;
        }

        public bool DoTurn()
        {
            Console.WriteLine($"-- {_attacker.Name} turn --");
            _player.PrintStats();
            _boss.PrintStats();

            var targetDied = _attacker.DoAction(_target);
            _gameEnded = targetDied;

            Console.WriteLine();
            if (!_gameEnded)
                SwitchAttackerTarget();
            return _gameEnded;
        }

        public bool PlayerWon()
        {
            if (!_gameEnded)
                throw new InvalidOperationException("Game has not ended yet.");
            return _boss.HitPoints <= 0 && _player.HitPoints > 0;
        }

        private void SwitchAttackerTarget() => (_attacker, _target) = (_target, _attacker);
    }

    public class Player : Entity
    {
        public Player(int hp, int mana) : base("Player", hp, mana)
        {
        }

        public int TotalManaSpend { get; private set; }

        public override bool DoAction(Entity target)
        {
            var spell = ChooseSpell(target);

            if (spell is not null)
                CastSpell(spell, target);

            return target.HitPoints <= 0;
        }

        private Spell? ChooseSpell(Entity target)
        {
            var spells = GetAvailableSpells(target);

            if (!spells.Any())
                return null;

            var random = new Random().Next(0, spells.Count - 1);
            return spells.ElementAt(random);
        }

        private ICollection<Spell> GetAvailableSpells(Entity target)
        {
            return s_spells.Where(s => s.ManaCost <= Mana &&
                (s.CanCast?.Invoke(this, target) ?? true)).ToList();
        }

        private void CastSpell(Spell spell, Entity target)
        {
            if (Mana < spell.ManaCost)
                throw new InvalidOperationException($"Cannot cast '{spell.Name}'. Not enough mana.");

            Console.WriteLine($"{Name} casts {spell.Name}");
            spell.Cast(this, target);

            IncreaseMana(-spell.ManaCost);
            TotalManaSpend += spell.ManaCost;
        }

        public override bool TakeDamage(int damage)
        {
            var damageTaken = damage - Armor;
            return base.TakeDamage(damageTaken);
        }

        public override void PrintStats() => Console.WriteLine($"- {Name} has {HitPoints} hit points, {Armor} armor, {Mana} mana");
    }

    public class Boss : Entity
    {
        public Boss(int hp, int damage) : base("Boss", hp)
        {
            Damage = damage;
        }

        public int Damage { get; }

        public override bool DoAction(Entity target) => target.TakeDamage(Damage);
    }

    public abstract class Entity
    {
        private readonly int _startingHp;
        private readonly int _startingMana;
        protected readonly List<Effect> _activeEffects = new();

        public Entity(string name, int hp, int mana = 0)
        {
            Name = name;
            HitPoints = hp;
            Mana = mana;

            _startingHp = hp;
            _startingMana = mana;
        }

        public string Name { get; }
        public int HitPoints { get; private set; }
        public int Mana { get; private set; }
        public int Armor { get; private set; }

        public abstract bool DoAction(Entity target);

        public void HandleEffects()
        {
            foreach (var effect in _activeEffects)
            {
                if (effect.EachTurn)
                    effect.OnActive(this);
                effect.DecreaseTurns();

                if (effect.Turns <= 0)
                    effect.OnDeactivate?.Invoke(this);
            }

            _activeEffects.RemoveAll(e => e.Turns <= 0);
        }

        /// <summary>
        /// Decrease hitpoints of the entity.
        /// </summary>
        /// <returns>When the entity died returns true; otherwise false.</returns>
        public virtual bool TakeDamage(int damage)
        {
            var damageTaken = damage < 1 ? 1 : damage;

            HitPoints -= damageTaken;

            return HitPoints <= 0;
        }
        public void Heal(int hp) => HitPoints += hp;
        public void IncreaseArmor(int armor) => Armor += armor;
        public void IncreaseMana(int mana) => Mana += mana;

        public void GiveEffect(Effect effect)
        {
            if (HasEffect(effect.Type))
                throw new InvalidOperationException($"Effect '{effect.Type}' is already active");

            _activeEffects.Add(effect);
        }
        public bool HasEffect(EffectType effectType) => _activeEffects.Any(e => e.Type == effectType);

        public virtual void PrintStats() => Console.WriteLine($"- {Name} has {HitPoints} hit points");
        public virtual void Reset()
        {
            HitPoints = _startingHp;
            Mana = _startingMana;
            Armor = 0;

            _activeEffects.Clear();
        }
    }

    public record Spell(
        string Name, 
        int ManaCost, 
        Action<Entity, Entity> Cast,
        Func<Entity, Entity, bool>? CanCast = null);

    public record Effect(EffectType Type, int Turns, Action<Entity> OnActive, Action<Entity>? OnDeactivate = null, bool EachTurn = true)
    {
        public int Turns { get; private set; }  = Turns;
        public int DecreaseTurns() => Turns--;

        /// <summary>
        /// +7 armor when active
        /// </summary>
        public static Effect Shield(int turns) => new(EffectType.Shield, turns, e => e.IncreaseArmor(+7), e => e.IncreaseArmor(-7), EachTurn: false);

        /// <summary>
        /// -3 HP each turn
        /// </summary>
        public static Effect Poison(int turns) => new(EffectType.Poison, turns, e => e.TakeDamage(3));

        /// <summary>
        /// +101 Mana each turn
        /// </summary>
        public static Effect Recharge(int turns) => new(EffectType.Recharge, turns, e => e.IncreaseMana(101));
    }

    public enum EffectType
    {
        Shield,
        Poison,
        Recharge,
    }
}
