using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace _1DV402.S2.L1C
{
    // Structs
    public enum Outcome
    { 
        Indefinite,
        Low,
        High,
        Right,
        NoMoreGuesses,
        OldGuess
    }

    public struct GuessedNumber
    {
        public int? Number;
        public Outcome Outcome;
    }

    public class SecretNumber
    {
        // Variables
        private GuessedNumber[] _guessedNumbers;
        private int? _number;
        public const int MaxNumberOfGuesses = 7;

        // Variables for Properties
        private int _count;
        private bool _canMakeGuess = true;

        private Outcome _outcome;

        // Properties
        public bool CanMakeGuess
        {
            /*
             *  Egenskap, av typen bool där get är publik och set är privat, som håller ordning på om 
             *  användaren kan gissa eller inte.
             *  Så länge användaren kan göra en gissning ska egenskapen ha värdet true. 
             *  Egenskapen ska ha värdet false då en användare förbrukat sju gissningar eller lyckats gissa rätt.
             */
            get
            {
                Console.WriteLine(Count + " of " + MaxNumberOfGuesses);
                if (Count >= MaxNumberOfGuesses) 
                {
                    Console.WriteLine("false!");
                    return false;
                }
                else if (_outcome == Outcome.Right)
                {
                    Console.WriteLine("false!");
                    return false;
                }
                return _canMakeGuess;
            }
            private set
            {
                _canMakeGuess = value;
            }
        }

        // Osäker på denna
        public int Count 
        {
            /*
             * Egenskap, av typen int, där get är publik och set är privat, 
             * som räknar antalet gjorda gissningar sedan det hemliga talet slumpades fram.
             */

            get
            {
                // return _guessedNumbers.Length;
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

        public GuessedNumber[] GuessedNumbers 
        {
            /*
             * ”Read-only”-egenskap, av typen GuessedNumber[]som ger en referens till en kopia 
             * av det privata fältet _guessedNumbers. OBS! För att undvika en ”privacy leak” får 
             * under inga omständigheter en referens till det privata fältet _guessedNumbers 
             * returneras av egenskapen.
             */

            get
            {
                // Because arrays cannot be made readonly, we have to pass it trough a ReadOnlyCollection.

                // Init a return copy (type GuessedNumber[]) and a ReadOnlyCollection.
                ReadOnlyCollection<GuessedNumber> _readOnlyCollection;

                // Put array of GuessedNumbers into ReadOnlyCollection
                _readOnlyCollection = Array.AsReadOnly(_guessedNumbers);

                // Fetch array from our ReadOnlyCollection and return it.
                return _readOnlyCollection.ToArray();
            }
        }

        public int? Number
        {
            /* 
             * Egenskap, av typen int?, där get är publik och set är privat, som innehåller 
             * det hemliga talet. Så länge som det går att gissa ska egenskapen ge värdet null.
             * Går det inte att gissa ska egenskapen ge värdet den privata egenskapen _number har.
             */

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
            /*
             * Egenskap, av typen OutCome, där get är publik och set är privat, som innehåller utfallet 
             * av den senaste gissningen. Innan någon gissning gjorts ska egenskapen ha standardvärdet 
             * Outcome.Indefinite. 
             */
            get
            {
                return (Count == 0 ? Outcome.Indefinite : _outcome); 
            }
            private set
            {
                _outcome = value;
            }
        }


        // Methods
        public SecretNumber()
        {
            /*
             * Konstruktorn har till uppgift att se till att ett SecretNumber-objekt 
             * är korrekt initierat. Det innebär att fält och egenskaper har blivit 
             * tilldelade lämpliga värden, vilket enklast görs genom att låta konstruktorn 
             * anropa metoden Initialize().
             * Konstruktorn ska även ansvara för att instansiera arrayen som håller ordning 
             * på gjorda gissningar.
             */
            // Get things up and running
            Initialize();         
        }

        public void Initialize()
        {
            /*
             * Publik metod som initierar klassens fält och egenskaper.
                • Arrayen _guessedNumbers refererar till ska återanvändas 
                    * och måste därför rensas på gjorda gissningar och elementen 
                    * initieras till standardvärdet för typen GuessedNumber.
                • Number ska tilldelas ett slumpat heltal i det slutna intervallet mellan 1 och 100.
                • Count ska tilldelas värdet 0.
                • Guess ska tilldelas värdet null.
                • Outcome ska tilldelas värdet Outcome.Indefinite.
             */


            // Check if array is decleraded from before
            if (_guessedNumbers != null)
            {
                // Clear _guessedNumbers
                Array.Clear(this._guessedNumbers, 0, this._guessedNumbers.Length);
            }
            else
            {
                // Initialize array for the first time.
                _guessedNumbers = new GuessedNumber[MaxNumberOfGuesses];
            }

            // Declare random number between 1 and 100.
            Random _random = new Random();
            Number = _random.Next(1, 100);

            // Set standard value for outcome
            Outcome = Outcome.Indefinite;

            // Set count to 0
            Count = 0;

            // Set guess to null
            Guess = null;

        }

        public Outcome MakeGuess(int guess)
        {
            /*
             * Publik metod som anropas för att göra en gissning av det hemliga talet.
             * Beroende om det gissade talets värde, som parametern number innehåller, 
             * är för högt, lågt eller överensstämmer med det hemliga talet ska lämpligt 
             * värde av typen Outcome returneras. En gissning på ett tidigare gissat tal 
             * ska inte räknas.
             * Om det vid anrop av metoden MakeGuess() skickas med ett argument som inte är
             * i det slutna intervallet mellan 1 och 100 ska metoden, efter att undersökt 
             * parameterns värde, kasta ett undantag av typen ArgumentOutRangeException.
             */

            const int MaxGuessValue = 100;
            const int MinGuessValue = 1;

            // Set Guess property.
            Guess = guess;
            
            // If value is out of range, throw exception
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

            // Increase Guesscount
            _count++;

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
            _guessedNumbers[_count - 1].Number = guess;
            _guessedNumbers[_count - 1].Outcome = Outcome;

            return Outcome;
        }
    }
}
