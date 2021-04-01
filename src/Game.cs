using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace PrimeService
{
    public class Game
    {
        private List<string> allWords;
        private Dictionary <int, Player> players;
        private string lastCorrectWord;
        private string lastWord;
        private const string  FIRST_LETTER = "a";
        private string word;
        public Player player;
        private int playerID = 0;
        private bool end = true;
        public Game()
        {
            allWords = new List<string>();
            lastCorrectWord = FIRST_LETTER;
            players = new Dictionary<int, Player>();
            // player = players[playerID];
        }
        private void AddWordToAllWords()
        {
            allWords.Add(lastCorrectWord);
        }
        public void PlayerInputWord(string word)
        {
            CheckWord(word);
        }
        private void CheckWord(string word)
        {
            word = WordToLower(word);
            if (lastCorrectWord[lastCorrectWord.Length-1] == word[0] && lastCorrectWord != word && !allWords.Contains(word) && word != "-1")
            {
                this.word = word;
                lastWord = word;
                ChangeWords();
            }
            else
            {
                lastWord = word;
            }
        }
        private void ChangeWords()
        {
            lastCorrectWord = word;
            AddWordToAllWords();
        }
        public List<string> GetAllWords()
        {
            return allWords;
        }
        private string WordToLower(string word)
        {
            return word.ToLower();
        }
        public string GetLastCorrectWord()
        {
            return lastCorrectWord;
        }
        private void CenenctPlayerToGame(Player player)
        {
            playerID++;
            players.Add(playerID, player);
            this.player = players[playerID];
        }
        private void GoToNextPlayer()
        {
            playerID++;
            if (players.ContainsKey(playerID))
            {
                player = players[playerID];
            }
            else
            {
                playerID = 1;
                player = players[playerID];
            }
        }
        private bool CloseGame()
        {
            end = false;
            return end;
        }
        public void CreateRoomForSomePlayer(int numOfPlayers, TcpListener server)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                TcpClient client = server.AcceptTcpClient();
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.AutoFlush = true;
                StreamReader reader = new StreamReader(client.GetStream());
                writer.WriteLine("Input your name:");
                CenenctPlayerToGame(new Player(reader.ReadLine(),client.GetStream()));
            }
            StartGame();
        }
        private void MessageToEveryone(string msg)
        {
            foreach(var player in players)
                {
                    StreamWriter writerToAllPlayers = new StreamWriter(player.Value.client);
                    writerToAllPlayers.AutoFlush = true;
                    writerToAllPlayers.WriteLine(msg);
                }
        }
        private void StartGame()
        {
            while (end)
            {
                StreamReader reader;
                StreamWriter writer;
                reader = new StreamReader(player.client);
                writer = new StreamWriter(player.client);
                writer.AutoFlush = true;
                writer.WriteLine($"Ваша черга ввести слово на букву {GetLastCorrectWord()[GetLastCorrectWord().Length - 1]}");
                string word =reader.ReadLine();  
                if (word == "-1")
                {
                    CloseGame();
                }
                else
                {
                    PlayerInputWord(word);
                    MessageToEveryone($"{player.GetName()} написав слово {lastWord}");
                }
            }
            Console.WriteLine("Game Over!");
            foreach(var player in players)
            {
                MessageToEveryone($"{player.Value.GetName()} програв)");
                player.Value.client.Close();
            }
        }
    }
}
