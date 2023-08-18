namespace AdventOfCode._2015;

[Challenge(2015, 21)]
internal class Day21_RPG_Simulator_20XX
{
    private static readonly Item[] s_items = new Item[]
    {
        // Weapons:
        new Weapon("Dagger",     cost: 8,  damage: 4, armor: 0),
        new Weapon("Shortsword", cost: 10, damage: 5, armor: 0),
        new Weapon("Warhammer",  cost: 25, damage: 6, armor: 0),
        new Weapon("Longsword",  cost: 40, damage: 7, armor: 0),
        new Weapon("Greataxe",   cost: 74, damage: 8, armor: 0),

        // Armor:
        new Armor("No Armor",   cost: 0,   damage: 0, armor: 0),
        new Armor("Leather",    cost: 13,  damage: 0, armor: 1),
        new Armor("Chainmail",  cost: 31,  damage: 0, armor: 2),
        new Armor("Splintmail", cost: 53,  damage: 0, armor: 3),
        new Armor("Bandedmail", cost: 75,  damage: 0, armor: 4),
        new Armor("Platemail",  cost: 102, damage: 0, armor: 5),

        // Rings:
        new Ring("No Ring",    cost: 0,   damage: 0, armor: 0),
        new Ring("No Ring",    cost: 0,   damage: 0, armor: 0),
        new Ring("Damage +1",  cost: 25,  damage: 1, armor: 0),
        new Ring("Damage +2",  cost: 50,  damage: 2, armor: 0),
        new Ring("Damage +3",  cost: 100, damage: 3, armor: 0),
        new Ring("Defense +1", cost: 20,  damage: 0, armor: 1),
        new Ring("Defense +2", cost: 40,  damage: 0, armor: 2),
        new Ring("Defense +3", cost: 80,  damage: 0, armor: 3),
    };

    [Part(1)]
    public static void Part01()
    {
        var player = new Player(hp: 100);
        var lowestCost = -1;

        foreach (var weapon in s_items.Where(i => i.Type is ItemType.Weapon).Cast<Weapon>())
        {
            player.PickWeapon(weapon);

            foreach (var armor in s_items.Where(i => i.Type is ItemType.Armor).Cast<Armor>())
            {
                player.PickArmor(armor);

                foreach (var leftRing in s_items.Where(i => i.Type is ItemType.Ring).Cast<Ring>())
                {
                    player.PickLeftRing(leftRing);

                    foreach (var rightRing in s_items.Where(i => i.Type is ItemType.Ring).Cast<Ring>())
                    {
                        if (leftRing == rightRing) continue;

                        player.PickRightRing(rightRing);

                        if (PlayGame(player))
                        {
                            var cost = player.GetGearCost();
                            if (lowestCost < 0 || lowestCost > cost)
                                lowestCost = cost;
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Player can win with only spending {lowestCost}");
    }

    [Part(2)]
    public static void Part02()
    {
        var player = new Player(hp: 100);
        var highestCost = -1;

        foreach (var weapon in s_items.Where(i => i.Type is ItemType.Weapon).Cast<Weapon>())
        {
            player.PickWeapon(weapon);

            foreach (var armor in s_items.Where(i => i.Type is ItemType.Armor).Cast<Armor>())
            {
                player.PickArmor(armor);

                foreach (var leftRing in s_items.Where(i => i.Type is ItemType.Ring).Cast<Ring>())
                {
                    player.PickLeftRing(leftRing);

                    foreach (var rightRing in s_items.Where(i => i.Type is ItemType.Ring).Cast<Ring>())
                    {
                        if (leftRing == rightRing) continue;

                        player.PickRightRing(rightRing);

                        if (!PlayGame(player))
                        {
                            var cost = player.GetGearCost();
                            if (highestCost < 0 || highestCost < cost)
                                highestCost = cost;
                        }
                    }
                }
            }
        }

        Console.WriteLine($"Boss can even win when the player spended {highestCost}");
    }

    public static bool PlayGame(Player player)
    {
        player.ResetHitPoints();

        var boss = new Boss(hp: 104, damage: 8, armor: 1);
        var game = new Game(player, boss);

        while (true)
        {
            if (game.DoTurn())
                break;
        }

        var playerWon = game.PlayerWon();

        return playerWon;
    }

    public class Game
    {
        private readonly Player _player;
        private readonly Boss _boss;

        private Entity _attacker;
        private Entity _defender;

        private bool _gameEnded = false;

        public Game(Player player, Boss boss)
        {
            _player = player;
            _boss = boss;

            _attacker = _player;
            _defender = _boss;
        }

        public bool PlayerWon()
        {
            if (!_gameEnded)
                throw new InvalidOperationException("Game has not ended yet.");
            return _boss.HitPoints <= 0 && _player.HitPoints > 0;
        }

        public bool DoTurn()
        {
            if (_gameEnded)
                throw new InvalidOperationException("Game has ended.");

            var defenderDied = _defender.TakeDamage(_attacker.Damage);

            if (!defenderDied)
                SwitchAttackerDefender();

            _gameEnded = defenderDied;
            return defenderDied;
        }

        private void SwitchAttackerDefender()
        {
            (_defender, _attacker) = (_attacker, _defender);
        }
    }

    public class Player : Entity
    {
        public Player(int hp) : base("Player", hp, 0, 0) { }

        public Weapon? Weapon { get; private set; }
        public Armor? ArmorGear { get; private set; }
        public Ring? RingLeftHand { get; private set; }
        public Ring? RingRightHand { get; private set; }

        public void PickWeapon(Weapon weapon)
        {
            Weapon = weapon;
            SetPlayerStats();
        }

        public void PickArmor(Armor armor)
        {
            ArmorGear = armor;
            SetPlayerStats();
        }

        public void PickLeftRing(Ring ring)
        {
            RingLeftHand = ring;
            SetPlayerStats();
        }

        public void PickRightRing(Ring ring)
        {
            RingRightHand = ring;
            SetPlayerStats();
        }

        public int GetGearCost()
        {
            var cost = 0;

            cost += Weapon?.Cost ?? 0;
            cost += ArmorGear?.Cost ?? 0;
            cost += RingLeftHand?.Cost ?? 0;
            cost += RingRightHand?.Cost ?? 0;

            return cost;
        }

        private void SetPlayerStats()
        {
            SetPlayerDamage();
            SetPlayerArmor();
        }

        private void SetPlayerDamage()
        {
            var damage = 0;
            damage += Weapon?.Damage ?? 0;
            damage += ArmorGear?.Damage ?? 0;
            damage += RingLeftHand?.Damage ?? 0;
            damage += RingRightHand?.Damage ?? 0;

            Damage = damage;
        }

        private void SetPlayerArmor()
        {
            var armor = 0;
            armor += Weapon?.Armor ?? 0;
            armor += ArmorGear?.Armor ?? 0;
            armor += RingLeftHand?.Armor ?? 0;
            armor += RingRightHand?.Armor ?? 0;

            Armor = armor;
        }

    }

    public class Boss : Entity
    {
        public Boss(int hp, int damage, int armor) : base("Boss", hp, damage, armor) { }
    }

    public class Entity
    {
        private readonly int _startingHp;

        public Entity(string name, int hp, int damage, int armor)
        {
            Name = name;
            HitPoints = hp;
            _startingHp = hp;
            Damage = damage;
            Armor = armor;
        }

        public string Name { get; set; }
        public int HitPoints { get; private set; }
        public int Damage { get; protected set; }
        public int Armor { get; protected set; }

        public void ResetHitPoints() => HitPoints = _startingHp;

        public bool TakeDamage(int damage)
        {
            var damageTaken = damage - Armor;
            damageTaken = damageTaken <= 0 ? 1 : damageTaken;

            HitPoints -= damageTaken;

            return HitPoints <= 0;
        }
    }

    public record Item(ItemType Type, string Name, int Cost, int Damage, int Armor);

    public record class Weapon : Item
    {
        public Weapon(string name, int cost, int damage, int armor)
            : base(ItemType.Weapon, name, cost, damage, armor)
        {
        }
    }

    public record class Armor : Item
    {
        public Armor(string name, int cost, int damage, int armor)
            : base(ItemType.Armor, name, cost, damage, armor)
        {
        }
    }

    public record class Ring : Item
    {
        public Ring(string name, int cost, int damage, int armor)
            : base(ItemType.Ring, name, cost, damage, armor)
        {
        }
    }

    public enum ItemType
    {
        Weapon,
        Armor,
        Ring
    }
}
