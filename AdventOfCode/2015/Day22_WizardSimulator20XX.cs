namespace AdventOfCode._2015;
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
        var minimalManaUsed = -1;

        while (true)
        {
            var game = new Game();

            while (true)
            {
                if (game.DoTurn()) break;
            }

            var playerWon = game.PlayerWon(out int totalMana);
            Console.WriteLine($"player won: {playerWon}, mana: {totalMana}, turns: {game.Turns}");
            Console.WriteLine();
            if (playerWon)
            {
                minimalManaUsed = minimalManaUsed == -1 ? totalMana : 
                    (minimalManaUsed > totalMana) ? totalMana : minimalManaUsed;
            }
        }
    }

    public class Game
    {
        private readonly Player _player = new(50, 500);
        private readonly Boss _boss = new(71, 10);

        private Entity _target;
        private Entity _attacker;

        private bool _gameEnded;

        public int Turns { get; private set; }

        public Game()
        {
            _target = _boss;
            _attacker = _player;
        }

        public bool DoTurn()
        {
            Console.WriteLine($"-- {_attacker.Name} turn [{++Turns}]--");
            _player.PrintStats();
            _boss.PrintStats();

            if (_player.HandleEffects())
            {
                _gameEnded = true;
                return true;
            }

            if (_boss.HandleEffects())
            {
                _gameEnded = true;
                return true;
            }

            if (_attacker.DoAction(_target))
            {
                _gameEnded = true;
                return true;
            }

            Console.WriteLine();
            if (!_gameEnded)
                SwitchAttackerTarget();

            return false;
        }

        public bool PlayerWon(out int totalManaSpend)
        {
            if (!_gameEnded)
                throw new InvalidOperationException("Game has not ended yet.");
            totalManaSpend = _player.TotalManaSpend;
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

            var spell = ChooseSpellFromAvailableSpellUsingPlayerInput(spells.ToArray(), this, target);

            return spell;
        }

        private static Spell ChooseSpellFromAvailableSpellsUsingScore(Spell[] spells, Player player, Entity target)
        {
            var spell = spells.OrderByDescending(s => s.Score)
                .ThenBy(s => s.ManaCost)
                .FirstOrDefault();
            return spell!;
        }

        private static Spell ChooseSpellFromAvailableSpellUsingPlayerInput(Spell[] spells, Player player, Entity target)
        {
            for (int i = 0; i < spells.Length; i++)
                Console.WriteLine($"{i}: {spells[i].Name,15} {spells[i].ManaCost, 5}");

            var spellIndex = int.Parse(Console.ReadLine() ?? "");
            var spell = spells[spellIndex];

            return spell;
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

        public bool HandleEffects()
        {
            foreach (var effect in _activeEffects)
            {
                if (effect.EachTurn)
                {
                    Console.WriteLine($"{effect.Type} handled");
                    effect.OnActive(this);
                }
                else if (effect.FirstActivation)
                {
                    Console.WriteLine($"{effect.Type} handled");
                    effect.OnActive(this);
                }

                effect.DecreaseTurns();
                Console.WriteLine($"{effect.Type} {effect.Turns} turns left");

                if (effect.Turns <= 0)
                    effect.OnDeactivate?.Invoke(this);
            }

            _activeEffects.RemoveAll(e => e.Turns <= 0);

            return HitPoints <= 0;
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
        Func<Entity, Entity, bool>? CanCast = null, 
        int Score = 0);

    public record Effect(EffectType Type, int Turns, Action<Entity> OnActive, Action<Entity>? OnDeactivate = null, bool EachTurn = true)
    {
        private readonly int _startTurns = Turns;

        public int Turns { get; private set; }  = Turns;
        public int DecreaseTurns() => Turns--;
        public bool FirstActivation => Turns == _startTurns;

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
