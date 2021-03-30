using System;

namespace Banco
{
    public class Conta
    {
        private TipoConta TipoConta { get; set; }
        private double Saldo { get; set; }
        private double Credito { get; set; }
        private string Nome { get; set; }

        public Conta(
            TipoConta tipoConta,
            double saldo,
            double credito,
            string nome)
        {
            this.TipoConta = tipoConta;
            this.Saldo = saldo;
            this.Credito = credito;
            this.Nome = nome;

            ValidarAtributos();
        }

        public Resultado Sacar(double valorSaque)
        {
            if (valorSaque <= 0)
                return new Resultado {
                    Concluido = false,
                    Mensagem = "Valor invalido."
                };

            if (this.Saldo - valorSaque < (this.Credito * -1 ))
                return new Resultado {
                    Concluido = false,
                    Mensagem = "Saldo insuficiente."
                };

            this.Saldo -= valorSaque;

            return new Resultado {
                Concluido = true,
                Mensagem = $"Saldo atual da conta de { this.Nome } é { this.Saldo }"
            };
        }

        public Resultado Depositar(double valorDeposito)
        {
            if (valorDeposito <= 0)
                return new Resultado {
                    Concluido = false,
                    Mensagem = "Valor invalido."
                };
            
            this.Saldo += valorDeposito;

            return new Resultado {
                Concluido = true,
                Mensagem = $"Saldo atual da conta de { this.Nome } é { this.Saldo }"
            };
        }

        public Resultado Transferir(double valorTransferencia, Conta contaDestino)
        {
            if (contaDestino == this)
                return new Resultado {
                    Concluido = false,
                    Mensagem = "Conta origem deve ser diferente da conta destino."
                };

            Resultado resultadoSacar = Sacar(valorTransferencia);
            
            if (resultadoSacar.Concluido == false)
                return resultadoSacar;
    
            Resultado resultadoDepositar = contaDestino.Depositar(valorTransferencia);

            return new Resultado {
                Concluido = resultadoDepositar.Concluido,
                Mensagem = $"{ resultadoSacar.Mensagem }\r\n{ resultadoDepositar.Mensagem }"
            };
        }

        public override string ToString()
        {
            return 
                $"TipoConta: { this.TipoConta.ToString().PadRight(14) } | " + 
                $"Nome: { this.Nome.PadRight(35) } | " + 
                $"Saldo: { string.Format("{0:0.00}", this.Saldo).PadLeft(10) } | " + 
                $"Credito: { string.Format("{0:0.00}", this.Credito).PadLeft(10) }";
        }

        public void ValidarAtributos()
        {
            if (!Enum.IsDefined<TipoConta>(this.TipoConta))
                throw new ArgumentException("Tipo de conta invalido.");

            if (string.IsNullOrWhiteSpace(this.Nome))
                throw new ArgumentException("Nome deve ser preenchido.");

            if (this.Credito < 0)
                throw new ArgumentException("Credito deve ser maior ou igual a 0.");
        }
    }
}