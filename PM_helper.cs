using System;
using System.Collections.Generic;


namespace PasswordHelper 
{

    class PM_helper
    {
        private int master_key_length = 32;
        private Dictionary<char, int> symbolTable;
        public PM_helper() {
            symbolTable = new Dictionary<char, int>();
            GenerateSymbolTable();
        }

        public int Master_Key_Length
        {
            get { return master_key_length; }
            set { master_key_length = value; }
        }

        private void GenerateSymbolTable()
        {
            for(char c = 'A'; c <= 'Z'; c++)
            {
                symbolTable.Add(c, c - 'A' + 1);
            }

            for (char c = 'a'; c <= 'z'; c++)
            {
                symbolTable.Add(c, c - 'a' + 27);
            }

            for (char c = '0'; c <= '9'; c++)
            {
                symbolTable.Add(c, c - '0' + 53);
            }

            char[] specialCharacters = { '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '_', '+', '=', '{', '}', '[', ']', '|', ';', ':', ',', '<', '>', '.', '?', '/' };
            int charLen = 63;
            foreach(char c in specialCharacters)
            {
                symbolTable.Add(c, charLen);
                charLen++;
            }
        }
        public string GenerateMasterKey()
        {
            string master_key = "";
            Random rn = new Random();
            Dictionary<int, char> rev_lookup = CreateReverseLookup(symbolTable);

            for(int i = 0; i < master_key_length; i++)
            {
                int n = rn.Next(1, rev_lookup.Count);
                master_key += rev_lookup[n];

            }
            return master_key;
        }
        public string EncryptPassword(string password, string master_key, int password_length)
        {
            string encrypted_password = "";
            Dictionary<int, char> rev_lookup = CreateReverseLookup(symbolTable);
            if(master_key.Length != master_key_length)
            {
                throw new Exception("master_key_length didn't match");
            }
            if(password.Length != password_length)
            {
                throw new Exception("password length and password_length must be the same.");
            }

            for(int i = 0; i < password_length; i++)
            {
                int m_k_n = symbolTable[master_key[i]];
                if(m_k_n == 1)
                {
                    encrypted_password += password[i];
                    continue;
                }
                if(m_k_n == symbolTable.Count)
                {
                    encrypted_password += password[i];
                    continue;
                }
                if(m_k_n % 2 == 0)
                {
                    encrypted_password += rev_lookup[symbolTable[password[i]] - 1];
                }
                else
                {
                    encrypted_password += rev_lookup[symbolTable[password[i]] + 1];
                }
            }

            return encrypted_password;
        }

        public string DecryptPassword(string encrypted_password, string master_key, int password_length)
        {
            string decrypted_password = "";
            Dictionary<int, char> rev_lookup = CreateReverseLookup(symbolTable);
            if(master_key.Length != master_key_length)
            {
                throw new Exception("master_key_length didn't match");
            }
            if(encrypted_password.Length != password_length)
            {
                throw new Exception("password length and password_length must be the same.");
            }

            for(int i = 0; i < password_length; i++)
            {
                int m_k_n = symbolTable[master_key[i]];
                if(m_k_n == 1)
                {
                    decrypted_password += encrypted_password[i];
                    continue;
                }
                if(m_k_n == symbolTable.Count) {
                    decrypted_password += encrypted_password[i];
                    continue;
                }
                if (m_k_n % 2 == 0)
                {
                    decrypted_password += rev_lookup[symbolTable[encrypted_password[i]] + 1];
                }
                else
                {
                    decrypted_password += rev_lookup[symbolTable[encrypted_password[i]] - 1];

                }
            }

            return decrypted_password;

        }

        static Dictionary<int, char> CreateReverseLookup(Dictionary<char, int> symbolTable)
        {
            Dictionary<int, char> reverseLookup = new Dictionary<int, char>();

            foreach (var kvp in symbolTable)
            {
                reverseLookup[kvp.Value] = kvp.Key;
            }

            return reverseLookup;
        }
    };

}
