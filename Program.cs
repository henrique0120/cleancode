// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;

// Interface para envio de notificações
public interface INotificador
{
    void Enviar(string destinatario, string assunto, string mensagem);
}

// Implementação do envio de email
public class NotificadorEmail : INotificador
{
    public void Enviar(string destinatario, string assunto, string mensagem)
    {
        Console.WriteLine($"E-mail enviado para {destinatario}. Assunto: {assunto}");
    }
}

// Implementação do envio de SMS
public class NotificadorSMS : INotificador
{
    public void Enviar(string destinatario, string assunto, string mensagem)
    {
        Console.WriteLine($"SMS enviado para {destinatario}: {mensagem}");
    }
}

// Classe de Livro
public class Livro
{
    public string Titulo { get; set; }
    public string Autor { get; set; }
    public string ISBN { get; set; }
    public bool Disponivel { get; set; } = true;
}

// Classe de Usuário
public class Usuario
{
    public string Nome { get; set; }
    public int ID { get; set; }
}

// Classe de Empréstimo
public class Emprestimo
{
    public Livro Livro { get; set; }
    public Usuario Usuario { get; set; }
    public DateTime DataEmprestimo { get; set; }
    public DateTime DataDevolucaoPrevista { get; set; }
    public DateTime? DataDevolucaoEfetiva { get; set; }

    public double CalcularMulta()
    {
        if (DataDevolucaoEfetiva > DataDevolucaoPrevista)
        {
            TimeSpan atraso = DataDevolucaoEfetiva.Value - DataDevolucaoPrevista;
            return atraso.Days * 1.0;
        }
        return 0;
    }
}

// Gerenciador de Livros
public class GerenciadorLivros
{
    private List<Livro> livros = new List<Livro>();

    public void AdicionarLivro(string titulo, string autor, string isbn)
    {
        livros.Add(new Livro { Titulo = titulo, Autor = autor, ISBN = isbn });
        Console.WriteLine("Livro adicionado: " + titulo);
    }

    public Livro BuscarPorISBN(string isbn)
    {
        return livros.Find(l => l.ISBN == isbn);
    }
}

// Gerenciador de Usuários
public class GerenciadorUsuarios
{
    private List<Usuario> usuarios = new List<Usuario>();

    public void AdicionarUsuario(string nome, int id)
    {
        usuarios.Add(new Usuario { Nome = nome, ID = id });
        Console.WriteLine("Usuário adicionado: " + nome);
    }

    public Usuario BuscarPorID(int id)
    {
        return usuarios.Find(u => u.ID == id);
    }
}

// Gerenciador de Empréstimos
public class GerenciadorEmprestimos
{
    private List<Emprestimo> emprestimos = new List<Emprestimo>();
    private readonly INotificador notificador;

    public GerenciadorEmprestimos(INotificador notificador)
    {
        this.notificador = notificador;
    }

    public bool RealizarEmprestimo(Livro livro, Usuario usuario, int diasEmprestimo)
    {
        if (livro != null && usuario != null && livro.Disponivel)
        {
            livro.Disponivel = false;
            var emprestimo = new Emprestimo
            {
                Livro = livro,
                Usuario = usuario,
                DataEmprestimo = DateTime.Now,
                DataDevolucaoPrevista = DateTime.Now.AddDays(diasEmprestimo)
            };
            emprestimos.Add(emprestimo);

            notificador.Enviar(usuario.Nome, "Empréstimo Realizado", $"Você pegou emprestado o livro: {livro.Titulo}");
            return true;
        }
        return false;
    }

    public double RealizarDevolucao(Livro livro, Usuario usuario)
    {
        var emprestimo = emprestimos.Find(e => e.Livro == livro && e.Usuario == usuario && e.DataDevolucaoEfetiva == null);

        if (emprestimo != null)
        {
            emprestimo.DataDevolucaoEfetiva = DateTime.Now;
            livro.Disponivel = true;
            double multa = emprestimo.CalcularMulta();
            if (multa > 0)
            {
                notificador.Enviar(usuario.Nome, "Multa por Atraso", $"Você tem uma multa de R$ {multa}");
            }
            return multa;
        }
        return -1; // Código de erro
    }
}

// Classe Program para testar
class Program
{
    static void Main(string[] args)
    {
        var gerenciadorLivros = new GerenciadorLivros();
        var gerenciadorUsuarios = new GerenciadorUsuarios();
        var notificadorEmail = new NotificadorEmail();
        var gerenciadorEmprestimos = new GerenciadorEmprestimos(notificadorEmail);

        // Adicionar livros
        gerenciadorLivros.AdicionarLivro("Clean Code", "Robert C. Martin", "978-0132350884");
        gerenciadorLivros.AdicionarLivro("Design Patterns", "Erich Gamma", "978-0201633610");

        // Adicionar usuários
        gerenciadorUsuarios.AdicionarUsuario("João Silva", 1);
        gerenciadorUsuarios.AdicionarUsuario("Maria Oliveira", 2);

        // Realizar empréstimo
        var livro = gerenciadorLivros.BuscarPorISBN("978-0132350884");
        var usuario = gerenciadorUsuarios.BuscarPorID(1);
        gerenciadorEmprestimos.RealizarEmprestimo(livro, usuario, 7);

        // Realizar devolução (com atraso simulado)
        double multa = gerenciadorEmprestimos.RealizarDevolucao(livro, usuario);
        Console.WriteLine($"Multa por atraso: R$ {multa}");

        Console.ReadLine();
    }
}
