using System;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace WPF_Agenda
{
    public partial class MainWindow : Window
    {
        ObservableCollection<Contato> contatos = new ObservableCollection<Contato>();

        public MainWindow()
        {
            InitializeComponent();
            lstContatos.ItemsSource = contatos;
        }

        private void AdicionarContato_Click(object sender, RoutedEventArgs e)
        {
            string nome = txtNome.Text;
            string telefone = txtTelefone.Text;

            if (!string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(telefone))
            {
                contatos.Add(new Contato { Nome = nome, Telefone = telefone });
                LimparCampos();
            }
            else
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
            }
        }

        private void LimparCampos()
        {
            txtNome.Clear();
            txtTelefone.Clear();
        }

        private void SalvarContato_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection("sua_string_de_conexao"))
                {
                    connection.Open();

                    // Comando SQL para inserir dados na tabela Contatos
                    string sql = "INSERT INTO Contatos (Nome, Telefone) VALUES (@Nome, @Telefone)";

                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        // Parâmetros para evitar SQL Injection
                        cmd.Parameters.AddWithValue("@Nome", txtNome.Text);
                        cmd.Parameters.AddWithValue("@Telefone", txtTelefone.Text);

                        // Executa o comando SQL
                        cmd.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Contato salvo com sucesso no banco de dados!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar contato: {ex.Message}");
            }
        }

        private void VisualizarContatos_Click(object sender, RoutedEventArgs e)
        {
            // Obtenha o Frame associado à janela principal
            Frame frame = (Frame)Application.Current.MainWindow.Content;

            // Crie uma instância da página e navegue até ela
            ContatosPage contatosPage = new ContatosPage();
            frame.Navigate(contatosPage);
        }
    }
}
