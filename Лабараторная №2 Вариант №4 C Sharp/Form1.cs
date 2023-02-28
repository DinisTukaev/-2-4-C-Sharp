using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Лабараторная__2_Вариант__4_C_Sharp
{
    public partial class Form1 : Form
    {
        int questionCount; // счетчик вопросов
        int correctAnswers; // кол-во правильных ответов
        int wrongAnswers; // кол-во неправильных ответов
        string[] array; // для wrongAnswers
        int correctAnswersNumber; // номер правильного ответа
        int selectedResponse; // номер выбранного ответа
        System.IO.StreamReader read; // для считывания текстовой информации из файла
        string filePath = @"\test1.txt"; // путь к файлу с вопросами
        Label label = new Label(); // текст вопроса
        CheckBox[] checkBoxes = new CheckBox[5]; // варианты ответа
        Button[] button = new Button[2]; // кнопки для взаимодействия
        int i; // для цикла
        int numberOfQuestions; // кол-во вопросов, которые задал пользователь
        int numberOfPossibleAnswers; // кол-во вариантов ответа, которые задал пользователь
        int questionNumber; // номер вопроса в файле
        string[] massiv;

        public Form1()
        {
            InitializeComponent();
        }

        void start()
        {
            var encoding = System.Text.Encoding.GetEncoding(65001);

            try
            {
                read = new System.IO.StreamReader(System.IO.Directory.GetCurrentDirectory() + filePath, encoding);
                this.Text = read.ReadLine();
                questionCount = 0;
                correctAnswers = 0;
                wrongAnswers = 0;
                questionNumber = 0;
                array = new string[22];
                massiv= new string[numberOfQuestions+1];
            }
            catch(Exception)
            {
                MessageBox.Show("Ошибка открытия файла");
            }

            question();
        }

        void question() // смена вопросов
        {
            label.Text = read.ReadLine(); // считали вопрос
            checkBoxes[1].Text = read.ReadLine(); // считали вариант ответа
            checkBoxes[2].Text = read.ReadLine();
            checkBoxes[3].Text = read.ReadLine();
            checkBoxes[4].Text= read.ReadLine();
            correctAnswersNumber = int.Parse(read.ReadLine()); // считали правильный ответ
            questionNumber= int.Parse(read.ReadLine());

            checkBoxes[1].Checked = false;
            checkBoxes[2].Checked = false;
            checkBoxes[3].Checked = false;
            checkBoxes[4].Checked = false;
            button[0].Enabled = false;

            questionCount += 1;

            if (questionNumber == numberOfQuestions)
                    button[0].Text = "Завершить\nтестированиие";
        }

        void switchingStatus(object sender, EventArgs e)
        {
            button[0].Enabled = true;
            button[0].Focus();

            CheckBox checkBox = (CheckBox)sender;
            var value = checkBox.Name;

            selectedResponse = int.Parse(value.Substring(8));

            massiv[questionCount] = Convert.ToString(selectedResponse);
        }

        private void PressButton1(object sender, EventArgs e)
        {
            if (selectedResponse == correctAnswersNumber)
                correctAnswers += 1;

            if (selectedResponse != correctAnswersNumber)
            {
                wrongAnswers += 1;
                array[wrongAnswers] = label.Text;
            }

            if (button[0].Text == "Начать тестирование сначала")
            {
                button[0].Text = "Следующий вопрос";

                checkBoxes[1].Visible = true;
                checkBoxes[2].Visible = true;
                checkBoxes[3].Visible = true;
                checkBoxes[4].Visible = true;
                start();
                return;
            }

            if (button[0].Text == "Завершить\nтестированиие")
            {
                read.Close();

                checkBoxes[1].Visible = false;
                checkBoxes[2].Visible = false;
                checkBoxes[3].Visible = false;
                checkBoxes[4].Visible = false;

                label.Text = string.Format("Тестирование завершено.\n" +
                    "Правильных ответов: {0} из {1}\n" +
                    "Набранные баллы: {2:F2}", correctAnswers,
                    questionCount, (correctAnswers * 5.0f) / questionCount);

                button[0].Text = "Начать тестирование сначала";

                var inputIncorrectAnswers = "Список ошибок: \n\n";
                for (int i = 1; i <= wrongAnswers; i++)
                    inputIncorrectAnswers = inputIncorrectAnswers + array[i] + "\n";

                if (wrongAnswers != 0)
                    MessageBox.Show(inputIncorrectAnswers, "Тестирование завершено");

                SaveFileDialog saveFile1 = new SaveFileDialog();
                saveFile1.DefaultExt = "*.txt";
                saveFile1.Filter = "Text files|*.txt";
                if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
                {
                    using (StreamWriter sw = new StreamWriter(saveFile1.FileName, true))
                    {
                        sw.WriteLine("Ответы пользователя на тест по программированию");
                        for (int j = 1; j < numberOfQuestions+1; j++)
                            sw.WriteLine(/*$"{j}.",*/ massiv[j]);
                        sw.Close();
                    }
                }
            }

            if (button[0].Text == "Следующий\nвопрос")
                question();

        }

        private void PressButton2(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Controls.Clear();

            numberOfQuestions = Convert.ToInt32(textBox1.Text);
            numberOfPossibleAnswers= Convert.ToInt32(textBox2.Text);

            label.Location = new System.Drawing.Point(1, 10); // текст вопроса
            label.Size = new System.Drawing.Size(1000, 70);
            label.Font = new Font("Calibri", 14);
            Controls.Add(label);

            for (i = 1; i < checkBoxes.Length; i++) // варианты ответов
            {
                checkBoxes[i] = new CheckBox();
                checkBoxes[i].Location = new System.Drawing.Point(200, 28 + i * 50);
                checkBoxes[i].Size = new System.Drawing.Size(300, 50);
                checkBoxes[i].Name = "checkBox" + i.ToString();
                checkBoxes[i].Font = new Font("Calibri", 14);
                Controls.Add(checkBoxes[i]);
            }

            for (int i = 0; i < button.Length; i++)
            {
                button[i] = new Button();
                button[i].Location = new System.Drawing.Point(150 + i * 120, 300);
                button[i].Size = new System.Drawing.Size(110, 50);
                button[i].Font = new Font("Calibri", 10);
                Controls.Add(button[i]);
            }

            button[0].Click += new EventHandler(PressButton1);
            button[1].Click += new EventHandler(PressButton2);

            button[0].Text = "Следующий\nвопрос";
            button[1].Text = "Выход";

            checkBoxes[1].CheckedChanged += new EventHandler(switchingStatus);
            checkBoxes[2].CheckedChanged += new EventHandler(switchingStatus);
            checkBoxes[3].CheckedChanged += new EventHandler(switchingStatus);
            checkBoxes[4].CheckedChanged += new EventHandler(switchingStatus);

            if (numberOfPossibleAnswers == 3)
                checkBoxes[4].Visible = false;

            if (numberOfPossibleAnswers == 2)
            {
                checkBoxes[3].Visible = false;
                checkBoxes[4].Visible = false;
            }

            start();
        }
    }
}
