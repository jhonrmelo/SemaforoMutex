using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SemaforoMutexCSharp
{
    class Program
    {
        static bool podeAlocar = true;
        static void Main(string[] args)
        {
            Console.WriteLine("Digite as características dos processsos:");
            string inputControl = "";
            List<Processo> lstProcesso = new List<Processo>();
            string nome;
            int tur;
            do
            {
                Console.WriteLine("Digite o nome do processo:");
                nome = Console.ReadLine();

                Console.WriteLine("Digite  a quantidade de tempo do processo");
                tur = Convert.ToInt32(Console.ReadLine());

                lstProcesso.Add(new Processo(nome, tur));

                Console.WriteLine("Deseja parar a criação de processos? Se sim digite 'P', se não digite 'C'");
                inputControl = Console.ReadLine();

            } while (inputControl.ToUpper() != "P");


            while (_calculaTurs(lstProcesso) > 0)
            {
                Console.Clear();
                foreach (var processo in lstProcesso)
                {
                    _escreveProcessos(processo);
                    _down(processo);
                    _up(processo);
                }
                _mostraProcessoEmExecucao(lstProcesso);
                Thread.Sleep(1000);
            }
            Console.Clear();
            _escreveProcessos(lstProcesso);
            Console.ReadLine();
        }

        static void _escreveProcessos(Processo processo)
        {
            Console.WriteLine();
            Console.WriteLine($"Processo:  {processo.Nome} ");
            Console.WriteLine($"Tempo restante: {processo.Tur}");
            Console.WriteLine($"Status: {_showEnumString(processo.Status)}");
        }

        static void _escreveProcessos(List<Processo> lstProcesso)
        {
            foreach (var processo in lstProcesso)
            {
                _escreveProcessos(processo);
            }
        }
        static int _calculaTurs(List<Processo> lstSemaforo)
        {
            return lstSemaforo.Sum(semaforo => semaforo.Tur);
        }

        private static void _mostraProcessoEmExecucao(List<Processo> lstProcesso)
        {
            Processo processoEmExecucao = findByStatus(lstProcesso, EnumStatus.EmExecucao);

            if (processoEmExecucao != null)
                Console.WriteLine($"O processo em execução é o processo: {processoEmExecucao.Nome}");
            else
                Console.WriteLine("Não existe nenhum processo em execução!");
        }

        static string _showEnumString(EnumStatus enumStatus)
        {
            switch (enumStatus)
            {
                case EnumStatus.Dormindo:
                    return "Sleep";
                case EnumStatus.Livre:
                    return enumStatus.ToString();
                case EnumStatus.EmExecucao:
                    return "Em execução";
                case EnumStatus.Finalizado:
                    return enumStatus.ToString();
                default:
                    return "";
            }
        }

        static void _down(Processo processo)
        {
            bool emExecuxao = processo.Status == EnumStatus.EmExecucao;
            bool finalizado = processo.Status == EnumStatus.Finalizado;

            if (!emExecuxao && !finalizado)
                Console.WriteLine($"O processo - {processo.Nome} está solicitando um recurso.");

            if (podeAlocar && _isAlocavel(processo))
                processo.Status = EnumStatus.EmExecucao;

            else if (processo.Status == EnumStatus.Livre)
            {
                processo.Status = EnumStatus.Dormindo;
            }

            podeAlocar = false;
        }

        static Processo findByStatus(List<Processo> lstProcesso, EnumStatus enumStatus)
        {
            return lstProcesso.Where(Processo => Processo.Status == enumStatus).FirstOrDefault();
        }

        private static bool _isAlocavel(Processo processo)
        {
            if (processo.Status == EnumStatus.EmExecucao || processo.Status == EnumStatus.Finalizado)
                return false;

            return true;
        }
        static void _up(Processo processo)
        {
            if (processo.Status == EnumStatus.EmExecucao)
            {
                processo.DecrementaTur();
            }
            if (processo.Tur == 0)
            {
                processo.Status = EnumStatus.Finalizado;
                podeAlocar = true;
            }
        }
    }
}
