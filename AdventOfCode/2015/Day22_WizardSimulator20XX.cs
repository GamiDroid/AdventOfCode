using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode._2015;
[Challenge(2015, 22)]
internal class Day22_WizardSimulator20XX
{
    private readonly static Spell[] s_spells =
    {
        new("Magic Missile", ManaCost: 53, Cast: (g) => g.DamageBoss(4)),
        new("Drain", ManaCost: 73, Cast: (g) => { g.DamageBoss(2); g.HealPlayer(2); }),
        new("Shield", ManaCost: 113, Cast: (g) => g.SetEffectTimer("Shield", 6), CanCast: (g) => !g.IsEffectActive("Shield")),
        new("Poison", ManaCost: 173, Cast: (g) => g.SetEffectTimer("Poison", 6), CanCast: (g) => !g.IsEffectActive("Poison")),
        new("Recharge", ManaCost: 229, Cast: (g) => g.SetEffectTimer("Recharge", 5), CanCast :(g) => !g.IsEffectActive("Recharge"))
    };

    [Part(1)]
    public static void Part01()
    {
        SolveGame();
    }

    [Part(2)]
    public static void Part02()
    {
        SolveGame(hard: true);
    }

    public static void SolveGame(bool hard = false)
    {
        var queue = new PriorityQueue<GameState, int>();

        // CHALLENGE:
        var state = new GameState(playerHp: 50, playerMana: 500, bossHp: 71);
        var game = new Game(10, hard);

        queue.Enqueue(state, state.ManaUsed);

        while (queue.Count > 0)
        {
            state = queue.Dequeue();
            game.SetGameState(state);

            if (game.HandleTurn(out int spellCount) && game.BossDied())
                break;

            var newState = game.SaveGameState();

            if (newState.PlayerHp > 0)
                queue.Enqueue(newState, newState.ManaUsed);

            if (state.PlayerTurn)
            {
                var increm = state.IncrementSpellIndex();
                if (increm.SpellIndex < spellCount)
                    queue.Enqueue(increm, increm.ManaUsed);
            }
        }

        if (game.BossDied())
            Console.WriteLine($"Player won with {game.ManaUsed} mana");
        else
            Console.WriteLine("Queue is empty but the player did not win :(");
    }

    public class Game
    {
        private bool _playerTurn;
        private int _playerHp;
        private int _playerMana;
        private int _manaUsed;

        private int _bossHp;

        private int _timerShieldEffect;
        private int _timerPoisonEffect;
        private int _timerRechargeEffect;

        private int _spellIndex;

        private readonly int _bossStrength;

        private readonly bool _hardMode;

        public Game(int bossStrength, bool hard = false)
        {
            _bossStrength = bossStrength;
            _hardMode = hard;
        }

        public void SetGameState(GameState state)
        {
            _playerTurn = state.PlayerTurn;

            _playerHp = state.PlayerHp;
            _playerMana = state.PlayerMana;
            _manaUsed = state.ManaUsed;

            _bossHp = state.BossHp;

            _timerShieldEffect = state.TimerShieldEffect;
            _timerPoisonEffect = state.TimerPoisonEffect;
            _timerRechargeEffect = state.TimerRechargeEffect;

            _spellIndex = state.SpellIndex;
        }

        public GameState SaveGameState() => new()
        {
            PlayerTurn = _playerTurn,
            PlayerHp = _playerHp,
            PlayerMana = _playerMana,
            ManaUsed = _manaUsed,
            
            BossHp = _bossHp,

            TimerShieldEffect = _timerShieldEffect,
            TimerPoisonEffect = _timerPoisonEffect,
            TimerRechargeEffect = _timerRechargeEffect,

            SpellIndex = 0,
        };

        public int ManaUsed => _manaUsed;

        public int CountAvailableSpells() => GetAvailableSpells().Length;

        public bool HandleTurn(out int spellCount)
        {
            spellCount = 0;
            if (_hardMode && _playerTurn)
            {
                _playerHp--;
                if (_playerHp <= 0)
                    return true;
            }

            HandleEffects();

            if (_playerTurn) HandlePlayerTurn(out spellCount);
            else HandleBossTurn();

            var isEnded = IsEnded();

            if (!isEnded)
                ChangeTurns();
            return isEnded;
        }

        public void DamageBoss(int damage) => _bossHp -= damage;
        public void HealPlayer(int hp) => _playerHp += hp;

        public void SetEffectTimer(string effect, int timer)
        {
            var _ = effect.ToLowerInvariant() switch
            {
                "shield" => _timerShieldEffect = timer,
                "poison" => _timerPoisonEffect = timer,
                "recharge" => _timerRechargeEffect = timer,
                _ => throw new ArgumentException($"'{effect}' is no effect.", nameof(effect))
            };
        }

        public bool IsEffectActive(string effect)
        {
            return effect.ToLowerInvariant() switch
            {
                "shield" => _timerShieldEffect > 0,
                "poison" => _timerPoisonEffect > 0,
                "recharge" => _timerRechargeEffect > 0,
                _ => throw new ArgumentException($"'{effect}' is no effect.", nameof(effect))
            };
        }

        public bool IsEnded() => _playerHp <= 0 || _playerMana <= 0 || _bossHp <= 0;

        public bool BossDied() => _bossHp <= 0;

        private void HandleEffects()
        {
            if (_timerShieldEffect > 0)
                _timerShieldEffect--;

            if (_timerPoisonEffect > 0)
            {
                _bossHp -= 3;
               _timerPoisonEffect--;
            }

            if (_timerRechargeEffect > 0)
            {
                _playerMana += 101;
                _timerRechargeEffect--;
            }
        }

        private void HandlePlayerTurn(out int spellCount)
        {
            var availableSpells = GetAvailableSpells();
            spellCount = availableSpells.Length;

            if (_spellIndex >= availableSpells.Length)
                return;

            var spell = availableSpells[_spellIndex];

            spell.Cast(this);
            
            _playerMana -= spell.ManaCost;
            _manaUsed += spell.ManaCost;
        }

        private Spell[] GetAvailableSpells()
        {
            return s_spells.Where(s => s.ManaCost <= _playerMana && (s.CanCast?.Invoke(this) ?? true)).ToArray();
        }

        private void HandleBossTurn()
        {
            var armor = GetPlayerArmor();

            var damageTaken = _bossStrength - armor;
            damageTaken = damageTaken < 1 ? 1 : damageTaken;

            _playerHp -= damageTaken;
        }

        private int GetPlayerArmor()
        {
            return _timerShieldEffect > 0 ? 7 : 0;
        }

        private void ChangeTurns() => _playerTurn = !_playerTurn;
    }

    public readonly struct GameState
    {
        [SetsRequiredMembers]
        public GameState(int playerHp, int playerMana, int bossHp)
        {
            PlayerTurn = true;

            PlayerHp = playerHp;
            PlayerMana = playerMana;

            BossHp = bossHp;
        }

        public required bool PlayerTurn { get; init; }

        public required int PlayerHp { get; init; }
        public required int PlayerMana { get; init; }
        public required int ManaUsed { get; init; }

        public required int BossHp { get; init; }

        public required int TimerShieldEffect { get; init; }
        public required int TimerPoisonEffect { get; init; }
        public required int TimerRechargeEffect { get; init; }

        public required int SpellIndex { get; init; }

        public GameState IncrementSpellIndex() => new()
        {
            PlayerTurn = PlayerTurn,
            PlayerHp = PlayerHp,
            PlayerMana = PlayerMana,
            ManaUsed = ManaUsed,

            BossHp = BossHp,

            TimerShieldEffect = TimerShieldEffect,
            TimerPoisonEffect = TimerPoisonEffect,
            TimerRechargeEffect = TimerRechargeEffect,

            SpellIndex = SpellIndex+1,
        };
    }

    public record Spell(string Name, int ManaCost, Action<Game> Cast, Func<Game, bool>? CanCast = null);
}
