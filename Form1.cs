using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Wisielec_poprawny
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string password;
        public string visible_password = "";
        public string passwd_holder = "";
        public int tries = 5;

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show("Instrukcja: Celem gry jest odgadnięcie zamaskowanego hasła poprzez zgadywanie liter. Liczba możliwych błędów jest ograniczona. Miłej zabawy :) !");
            password = wybierzSlowo("slowa.txt");
            show_word(password);
        }

        public string wybierzSlowo(String filename)
        {
            int licznik = 1;
            System.IO.StreamReader sr = System.IO.File.OpenText(filename);
            while (!sr.EndOfStream)
            {
                String line = sr.ReadLine();
                for (int k = 0; k < line.Length; k++)
                {
                    if (line[k] == '|')
                    {
                        licznik++;
                    }
                }
                line = line.Trim();
                if (!String.IsNullOrEmpty(line))
                {
                    //haslo|haslo|haslo
                    String[] a = line.Split('|');
                    int rand = new Random().Next(0, licznik);
                    password = a[rand];
                }
            }
            sr.Close();
            return password;
        }

        private void show_word(string passwd)
        {
            for (int i = 0; i < passwd.Length; i++)
            {
                visible_password += "_";
                passwd_holder += "_";
                lbl_Passwd.Text += "_ ";
            }
        }

        private void check(KeyPressEventArgs key)
        {
            char[] password_lbl = lbl_Passwd.Text.ToCharArray();
            char[] visible_inChar = visible_password.ToCharArray();
            char[] holder_inChar = passwd_holder.ToCharArray();
            char pressed_key = key.KeyChar;

            if (password.Contains(pressed_key))
            {
                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] == pressed_key)
                    {
                        visible_inChar[i] = pressed_key;
                        string back = new string(visible_inChar);
                        for (int j = 0; j < password.Length; j++)
                        {
                            if (visible_inChar[j] != '_' && holder_inChar[j] == '_')
                            {
                                holder_inChar[j] = visible_inChar[j];
                            }
                        }
                        passwd_holder = new string(holder_inChar);
                        lbl_Passwd.Text = passwd_holder;
                    }
                }
            }
        }

        private void final_test()
        {
            string temp;
            temp = lbl_Passwd.Text.ToString();
            if (temp == password)
            {
                MessageBox.Show("WYGRAŁEŚ !!!");
                Application.Exit();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= 97 && e.KeyChar <= 122)
            {
                MessageBox.Show("Wcisnąłeś klawisz: '" + e.KeyChar.ToString() + "' ");
                  check(e);
                if (!password.Contains(e.KeyChar))
                {
                    tries--;
                    MessageBox.Show(" Pozostało Ci: " + tries.ToString() + "prób");
                    if (tries == 0)
                    {
                        MessageBox.Show("Niestety, przegrałeś.");
                        Application.Exit();
                    }
                }
                final_test();
            }
        }
    }
}
