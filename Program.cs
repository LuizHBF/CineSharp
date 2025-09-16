using System;
using System.Collections.Generic;

// --- CLASSES (NOSSOS MOLDES DE DADOS) ---
// Estas classes devem ficar aqui, fora da classe Program.

public class Filme
{
    public string Titulo { get; set; }
    public int DuracaoEmMinutos { get; set; }
    public string Genero { get; set; }
}

public class Sala
{
    public int Numero { get; set; }
    public string Tipo { get; set; }
    public int Capacidade { get; set; }
}

public class Sessao
{
    public Filme FilmeDaSessao { get; set; }
    public Sala SalaDaSessao { get; set; }
    public string Horario { get; set; }
    public bool[,] Assentos { get; private set; }

    // Construtor: Roda sempre que uma nova Sessao é criada
    public Sessao(Filme filme, Sala sala, string horario)
    {
        FilmeDaSessao = filme;
        SalaDaSessao = sala;
        Horario = horario;

        // Define um layout padrão de 10 fileiras.
        int fileiras = 10;
        int poltronasPorFileira = sala.Capacidade / fileiras;
        
        // Inicializa o mapa de assentos com todos os lugares livres (false)
        this.Assentos = new bool[fileiras, poltronasPorFileira];
    }
}


// --- PROGRAMA PRINCIPAL ---
// A classe Program contém o ponto de início da execução (método Main).

class Program
{
    public static void Main(string[] args)
    {
        // --- CONFIGURAÇÃO INICIAL DO CINEMA ---

        List<Filme> filmes = new List<Filme>();
        filmes.Add(new Filme { Titulo = "Duna: Parte 2", DuracaoEmMinutos = 166, Genero = "Ficção Científica" });
        filmes.Add(new Filme { Titulo = "Oppenheimer", DuracaoEmMinutos = 180, Genero = "Drama Histórico" });
        filmes.Add(new Filme { Titulo = "Pobres Criaturas", DuracaoEmMinutos = 141, Genero = "Comédia Dramática" });

        Sala sala1 = new Sala { Numero = 1, Tipo = "2D", Capacidade = 50 };
        Sala sala2 = new Sala { Numero = 2, Tipo = "3D", Capacidade = 40 };

        List<Sessao> sessoes = new List<Sessao>();
        sessoes.Add(new Sessao(filmes[0], sala2, "20:00"));
        sessoes.Add(new Sessao(filmes[1], sala1, "21:00"));
        sessoes.Add(new Sessao(filmes[2], sala1, "18:30"));

        bool executando = true;
        while (executando)
        {
            Console.Clear();
            Console.WriteLine("--- Bem-vindo ao CineSharp ---");
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1 - Listar sessões disponíveis");
            Console.WriteLine("2 - Comprar ingresso");
            Console.WriteLine("0 - Sair");
            Console.WriteLine("-----------------------------");
            Console.Write("Digite a opção desejada: ");

            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                    Console.WriteLine("\n--- Sessões Disponíveis ---");
                    if (sessoes.Count == 0)
                    {
                        Console.WriteLine("Nenhuma sessão disponível no momento.");
                    }
                    else
                    {
                        foreach (Sessao sessao in sessoes)
                        {
                            string tituloFilme = sessao.FilmeDaSessao.Titulo;
                            int numeroSala = sessao.SalaDaSessao.Numero;
                            string tipoSala = sessao.SalaDaSessao.Tipo;
                            string horario = sessao.Horario;

                            Console.WriteLine($"Filme: {tituloFilme}");
                            Console.WriteLine($"Sala: {numeroSala} ({tipoSala})");
                            Console.WriteLine($"Horário: {horario}");
                            Console.WriteLine("--------------------------");
                        }
                    }
                    break;

                case "2":
                    Console.WriteLine("\n--- Comprar Ingresso ---");
                    Sessao sessaoEscolhida = null;
                    while (sessaoEscolhida == null)
                    {
                        Console.WriteLine("\nEscolha uma das sessões disponíveis:");
                        for (int i = 0; i < sessoes.Count; i++)
                        {
                            Console.WriteLine($"{i + 1} - {sessoes[i].FilmeDaSessao.Titulo} - Sala {sessoes[i].SalaDaSessao.Numero} ({sessoes[i].SalaDaSessao.Tipo}) - {sessoes[i].Horario}");
                        }
                        Console.Write("\nDigite o número da sessão: ");

                        if (int.TryParse(Console.ReadLine(), out int numeroSessao) && numeroSessao > 0 && numeroSessao <= sessoes.Count)
                        {
                            sessaoEscolhida = sessoes[numeroSessao - 1];
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida! Por favor, digite um número da lista.");
                        }
                    }

                    int fileiraEscolhida = -1, poltronaEscolhida = -1;
                    while (true)
                    {
                        Console.WriteLine("\n--- Mapa de Assentos (O = Livre, X = Ocupado) ---");
                        for (int i = 0; i < sessaoEscolhida.Assentos.GetLength(0); i++)
                        {
                            Console.Write($"Fileira {i + 1}: ");
                            for (int j = 0; j < sessaoEscolhida.Assentos.GetLength(1); j++)
                            {
                                Console.Write(sessaoEscolhida.Assentos[i, j] ? "[X] " : "[O] ");
                            }
                            Console.WriteLine();
                        }

                        Console.Write("\nDigite o número da fileira: ");
                        int.TryParse(Console.ReadLine(), out fileiraEscolhida);

                        Console.Write("Digite o número da poltrona (da esquerda para direita): ");
                        int.TryParse(Console.ReadLine(), out poltronaEscolhida);

                        if (fileiraEscolhida > 0 && fileiraEscolhida <= sessaoEscolhida.Assentos.GetLength(0) &&
                            poltronaEscolhida > 0 && poltronaEscolhida <= sessaoEscolhida.Assentos.GetLength(1))
                        {
                            if (sessaoEscolhida.Assentos[fileiraEscolhida - 1, poltronaEscolhida - 1])
                            {
                                Console.WriteLine("Este assento já está ocupado! Por favor, escolha outro.");
                            }
                            else
                            {
                                sessaoEscolhida.Assentos[fileiraEscolhida - 1, poltronaEscolhida - 1] = true;
                                Console.WriteLine("Assento reservado com sucesso!");
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Número de fileira ou poltrona inválido. Tente novamente.");
                        }
                    }

                    double precoFinal = 0;
                    if (sessaoEscolhida.SalaDaSessao.Tipo == "3D")
                    {
                        precoFinal = 38.00;
                    }
                    else
                    {
                        precoFinal = 30.00;
                    }

                    while (true)
                    {
                        Console.Write($"\nO ingresso é (1) Inteira [R${precoFinal:F2}] ou (2) Meia [R${precoFinal / 2:F2}]? Digite o número: ");
                        string tipoIngresso = Console.ReadLine();
                        if (tipoIngresso == "1")
                        {
                            break;
                        }
                        else if (tipoIngresso == "2")
                        {
                            precoFinal /= 2;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Opção inválida! Por favor, digite 1 ou 2.");
                        }
                    }

                    Console.WriteLine("\n--- Resumo da Compra ---");
                    Console.WriteLine($"Filme: {sessaoEscolhida.FilmeDaSessao.Titulo}");
                    Console.WriteLine($"Sala: {sessaoEscolhida.SalaDaSessao.Numero} ({sessaoEscolhida.SalaDaSessao.Tipo})");
                    Console.WriteLine($"Horário: {sessaoEscolhida.Horario}");
                    Console.WriteLine($"Assento: Fileira {fileiraEscolhida}, Poltrona {poltronaEscolhida}");
                    Console.WriteLine($"Total a pagar: R$ {precoFinal:F2}");

                    break;

                case "0":
                    executando = false;
                    Console.WriteLine("\nObrigado por usar o CineSharp!");
                    break;

                default:
                    Console.WriteLine("\nOpção inválida! Tente novamente.");
                    break;
            }

            Console.WriteLine("\nPressione qualquer tecla para continuar...");
            Console.ReadKey();
        }
    }
}