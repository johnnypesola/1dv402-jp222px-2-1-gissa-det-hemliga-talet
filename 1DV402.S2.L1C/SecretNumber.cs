using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace _1DV402.S2.L1C
{
    public enum Outcome
    { 
        Indefinite,
        Low,
        High,
        Right,
        NoMoreGuesses,
        OldGuess
    }

    public class SecretNumber
    {
        // Variables
        private GuessedNumber[] _guessedNumbers;
        private int? _number;
        public const int MaxNumberOfGuesses = 7;

        // Variables for Properties
        private int _count;
        private bool _canMakeGuess;
        private Outcome _outcome;

        public bool CanMakeGuess
        {
            get
            {
                // Check if we are out of guesses, or the right number is guessed.
                if (Count >= MaxNumberOfGuesses || Outcome == Outcome.Right)  
                {
                    return false;
                }
                return _canMakeGuess;
            }
            private set
            {
                _canMakeGuess = value;
            }
        }

        public int Count 
        {
            get
            {
                return _count;
            }
            private set
            {
                // Increase count as long as it does not reach too high value
                if(_count >= MaxNumberOfGuesses)
                {
                    _count = 0; 
                }
                else
                {
                    _count = value;
                }
            }
        }

        public int? Guess { get; private set; }

        /// <summary>
        /// Gets a reference to a copy of the original.
        /// </summary>
        public GuessedNumber[] GuessedNumbers 
        {
            get
            {
                // Init a ReadOnlyCollection with GuessedNumber type.
                ReadOnlyCollection<GuessedNumber> _readOnlyCollection;

                // Put array of GuessedNumbers into ReadOnlyCollection
                _readOnlyCollection = Array.AsReadOnly(_guessedNumbers);

                // Fetch array from our ReadOnlyCollection and return it.
                return _readOnlyCollection.ToArray();
            }
        }

        public int? Number
        {
            get
            {
                return (CanMakeGuess ? null : _number);
            }
            private set
            {
                _number = value;
            }
        }

        public Outcome Outcome
        {
            get
            {
                return (Count == 0 ? Outcome.Indefinite : _outcome); 
            }
            private set
            {
                _outcome = value;
            }
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SecretNumber()
        {
            // Get things up and running, or reset this object
            Initialize();         
        }

        /// <summary>
        /// Resets this object to default values.
        /// </summary>
        public void Initialize()
        {
            Random _random = new Random();

            // Declare random number between 1 and 100.
            Number = _random.Next(1, 100);

            // Set standard values for object properties.
            Outcome = Outcome.Indefinite;
            Count = 0;
            Guess = null;
            CanMakeGuess = true;

            // Check if array is decleraded from before
            if (_guessedNumbers != null)
            {
                // Clear _guessedNumbers
                Array.Clear(_guessedNumbers, 0, _guessedNumbers.Length);
            }
            else
            {
                // Initialize array for the first time.
                _guessedNumbers = new GuessedNumber[MaxNumberOfGuesses];
            }
        }

        /// <summary>
        /// Makes guess and processes guess value.
        /// </summary>
        /// <param name="guess">Guessed number</param>
        /// <returns>Result of guessed number</returns>
        public Outcome MakeGuess(int guess)
        {
            const int MaxGuessValue = 100;
            const int MinGuessValue = 1;

            // Set Guess property.
            Guess = guess;

            // Throw an exception if value is out of range 
            if (guess > MaxGuessValue || guess < MinGuessValue)
            {
                throw new ArgumentOutOfRangeException();
            }

            // Check if more guesses cant be made.
            if (!CanMakeGuess)
            {
                Outcome = Outcome.NoMoreGuesses;
                return Outcome; // Abort
            }

            // Increase Guesscount by one
            Count += 1;

            // Check if value is the right value, too low or too high
            if (guess == _number)
            {
                Outcome = Outcome.Right;
            }
            else if ( guess < _number)
            {
                Outcome = Outcome.Low;
            }
            else if (guess > _number)
            {
                Outcome = Outcome.High;
            }

            // Check if number has been used in guess once allready.
            foreach (GuessedNumber guessed in _guessedNumbers)
            {
                if (guessed.Number == guess)
                {
                    Outcome = Outcome.OldGuess;
                }
            }

            // Store this guess
            _guessedNumbers[Count - 1].Number = guess;
            _guessedNumbers[Count - 1].Outcome = Outcome;

            return Outcome;
        }
    }
}
