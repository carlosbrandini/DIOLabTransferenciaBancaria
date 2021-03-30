using System;
using System.Collections.Generic;

namespace Banco
{
    public class Program
    {
        private static List<Conta> listContas = new List<Conta>();
        
        public static void Main(string[] args)
        {
			try
			{
				Processar();
			}
			catch
			{
				Console.WriteLine("Ocorreu um erro inesperado.");
			}
		}

		private static void Processar()
		{
			string opcaoUsuario = ObterOpcaoUsuario();

			while (opcaoUsuario != "X")
			{
				try
				{
					switch (opcaoUsuario)
					{
						case "1":
							ListarContas();
							break;
						case "2":
							InserirConta();
							break;
						case "3":
							Transferir();
							break;
						case "4":
							Sacar();
							break;
						case "5":
							Depositar();
							break;
						default:
							Console.WriteLine("Opcao invalida.");
							break;
					}

				}
				catch (ArgumentException argumentEx)
				{
					Console.WriteLine($"Erro: { argumentEx.Message }");
				}
				catch
				{
					Console.WriteLine("Ocorreu um erro inesperado. Tente Novamente.");
				}

				Console.WriteLine();
				Console.WriteLine("Aperte qualquer tecla para continuar.");
				Console.ReadKey();
				Console.Clear();

				opcaoUsuario = ObterOpcaoUsuario();
			}
			
			Console.WriteLine();
			Console.WriteLine("Obrigado por utilizar nossos serviços.");
			Console.ReadKey();
		}

        private static string ObterOpcaoUsuario()
		{
			Console.WriteLine();
			Console.WriteLine("Informe a opção desejada:");
			Console.WriteLine();
			Console.WriteLine("1- Listar contas");
			Console.WriteLine("2- Inserir nova conta");
			Console.WriteLine("3- Transferir");
			Console.WriteLine("4- Sacar");
			Console.WriteLine("5- Depositar");
			Console.WriteLine("X- Sair");
			Console.WriteLine();

			string opcaoUsuario = Console.ReadLine() ?? "";

			Console.Clear();

			return opcaoUsuario.ToUpper();
		}

        private static void ListarContas()
		{
			Console.WriteLine("*** Listar contas ***");
			Console.WriteLine();

			if (listContas.Count == 0)
			{
				Console.WriteLine("Nenhuma conta cadastrada.");
				return;
			}

			for (int i = 0; i < listContas.Count; i++)
				Console.WriteLine($"#{ i } - { listContas[i] }");
		}

        private static void InserirConta()
		{
			Console.WriteLine("*** Inserir nova conta ***");
			Console.WriteLine();

			Console.Write("Digite 1 para Conta Fisica ou 2 para Juridica: ");
			byte.TryParse(
				Console.ReadLine(),
				out byte entradaTipoConta);

			Console.Write("Digite o Nome do Cliente: ");
			string entradaNome = Console.ReadLine();

			Console.Write("Digite o saldo inicial: ");
			double entradaSaldo = LerDouble();

			Console.Write("Digite o credito: ");
			double entradaCredito = LerDouble();

			Conta novaConta = new Conta(
				tipoConta: (TipoConta)entradaTipoConta,
				saldo: entradaSaldo,
				credito: entradaCredito,
				nome: entradaNome);

			listContas.Add(novaConta);
		}

        private static void Transferir()
		{
			Console.WriteLine("*** Transferencia entre contas ***");
			Console.WriteLine();

			Console.Write("Digite o número da conta de origem: ");
			int indiceContaOrigem = LerIndiceConta();

            Console.Write("Digite o número da conta de destino: ");
			int indiceContaDestino = LerIndiceConta();

			Console.Write("Digite o valor a ser transferido: ");
			double valorTransferencia = LerDouble();
			
			TratarResultado(
				listContas[indiceContaOrigem].Transferir(
					valorTransferencia, 
					listContas[indiceContaDestino])
			);
		}

        private static void Sacar()
		{
			Console.WriteLine("*** Saque ***");
			Console.WriteLine();

			Console.Write("Digite o número da conta: ");
			int indiceConta = LerIndiceConta();

			Console.Write("Digite o valor a ser sacado: ");
			double valorSaque = LerDouble();

			TratarResultado(
            	listContas[indiceConta].Sacar(valorSaque));
		}

        private static void Depositar()
		{
			Console.WriteLine("*** Deposito ***");
			Console.WriteLine();

			Console.Write("Digite o número da conta: ");
			int indiceConta = LerIndiceConta();

			Console.Write("Digite o valor a ser depositado: ");
			double valorDeposito = LerDouble();

			TratarResultado(
            	listContas[indiceConta].Depositar(valorDeposito));
		}

		private static double LerDouble()
		{
			if(!double.TryParse(
				Console.ReadLine(), 
				out double entrada))
			{
				throw new ArgumentException("Valor deve ser numerico.");
			}

			return entrada;
		}

		private static int LerIndiceConta()
		{
			if (!int.TryParse(
				Console.ReadLine(), 
				out int indiceConta) ||
				indiceConta < 0)
			{
				throw new ArgumentException("Numero da conta invalido.");
			}

			if (indiceConta >= listContas.Count)
			{
				throw new ArgumentException("Numero da conta não cadastrado.");
			}

			return indiceConta;
		}

		private static void TratarResultado(Resultado resultado)
		{
			if (resultado.Concluido && !string.IsNullOrWhiteSpace(resultado.Mensagem))
				Console.WriteLine(resultado.Mensagem);
			else
				Console.WriteLine($"Erro: { resultado.Mensagem }");
		}
    }
}
