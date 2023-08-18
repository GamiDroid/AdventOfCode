namespace AdventOfCode._2015;
[Challenge(2015, 22)]
internal class Day22_WizardSimulator20XX
{
    [Part(1)]
    public void Part01()
    {
        var game = new Game();


    }

    public class Game
    {


        public void DoTurn()
        {
        }
    }

    public class Player : Entity
    {
        private readonly int _startingMana;

        public Player(int hp, int mana) : base("Player", hp)
        {
            Mana = mana;
            _startingMana = mana;
        }

        public int Mana { get; private set; }

        /// <summary>
        /// Reset hitpoints and mana.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            Mana = _startingMana;
        }
    }

    public class Boss : Entity
    {
        public Boss(int hp, int damage) : base("Boss", hp)
        {
            Damage = damage;
        }

        public int Damage { get; }
    }

    public class Entity
    {
        private readonly int _startingHp;

        public Entity(string name, int hp)
        {
            Name = name;
            HitPoints = hp;

            _startingHp = hp;
        }

        public string Name { get; }
        public int HitPoints { get; private set; }

        /// <summary>
        /// Reset the hitpoints back to the starting hitpoints.
        /// </summary>
        public virtual void Reset() => HitPoints = _startingHp;
    }
}
