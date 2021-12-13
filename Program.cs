// See https://aka.ms/new-console-template for more information
// Console.WriteLine("Hello, World!");
// Program.cs
// The Main() method

static IEnumerable<string> Suits()
{
    yield return "clubs";
    yield return "diamonds";
    yield return "hearts";
    yield return "spades";
}

static IEnumerable<string> Ranks()
{
    yield return "two";
    yield return "three";
    yield return "four";
    yield return "five";
    yield return "six";
    yield return "seven";
    yield return "eight";
    yield return "nine";
    yield return "ten";
    yield return "jack";
    yield return "queen";
    yield return "king";
    yield return "ace";
}


// ------------EJEMPLO 1 ---------------
//  var startingDeck = from s in Suits()
//                        from r in Ranks()
//                        select new { Suit = s, Rank = r };

//     // Display each card that we've generated and placed in startingDeck in the console
//     foreach (var card in startingDeck)
//     {
//         Console.WriteLine(card);
//     }
//----------------------------------------
// ---------------EJEMPLO 2-----------------
// var startingDeck = Suits().SelectMany(suit => Ranks().Select(rank => new { Suit = suit, Rank = rank }));
// foreach (var card in startingDeck)
//      {
//          Console.WriteLine(card);
//      }
//--------------------------------------------
// -------- MANIPULAR EL PEDIDO --------------
//   var startingDeck = from s in Suits()
//                        from r in Ranks()
//                        select new { Suit = s, Rank = r };

//     foreach (var c in startingDeck)
//     {
//         Console.WriteLine(c);
//     }

//     // 52 cards in a deck, so 52 / 2 = 26
//     var top = startingDeck.Take(26);
//     var bottom = startingDeck.Skip(26);
//     var shuffle = top.InterleaveSequenceWith(bottom);
//     //BARAJADO OPTIMIZADO
//     shuffle = shuffle.Skip(26).InterleaveSequenceWith(shuffle.Take(26));

//      foreach (var c in shuffle)
//     {
//         Console.WriteLine(c);
//     }

//     //-------- COMPARACIONES ------------

//      var times = 0;
//     // We can re-use the shuffle variable from earlier, or you can make a new one
//     shuffle = startingDeck;
//     do
//     {
//         shuffle = shuffle.Take(26).InterleaveSequenceWith(shuffle.Skip(26));

//         foreach (var card in shuffle)
//         {
//             Console.WriteLine(card);
//         }
//         Console.WriteLine();
//         times++;

//     } while (!startingDeck.SequenceEquals(shuffle));

//     Console.WriteLine(times);
//
// ------------- OPTIMIZADO--------------
   var startingDeck = (from s in Suits().LogQuery("Suit Generation")
                        from r in Ranks().LogQuery("Value Generation")
                        select new { Suit = s, Rank = r })
                        .LogQuery("Starting Deck")
                        .ToArray();

    foreach (var c in startingDeck)
    {
        Console.WriteLine(c);
    }

    Console.WriteLine();

    var times = 0;
    var shuffle = startingDeck;

    do
    {
        
        // shuffle = shuffle.Take(26)
        //     .LogQuery("Top Half")
        //     .InterleaveSequenceWith(shuffle.Skip(26).LogQuery("Bottom Half"))
        //     .LogQuery("Shuffle")
        //     .ToArray();
        

        shuffle = shuffle.Skip(26)
            .LogQuery("Bottom Half")
            .InterleaveSequenceWith(shuffle.Take(26).LogQuery("Top Half"))
            .LogQuery("Shuffle")
            .ToArray();

        foreach (var c in shuffle)
        {
            Console.WriteLine(c);
        }

        times++;
        Console.WriteLine(times);
    } while (!startingDeck.SequenceEquals(shuffle));

    Console.WriteLine(times);