using System;
using System.Collections.Generic;
using System.Linq;

namespace SemaforoMutexCSharp
{
    class Program
    {
        static bool podeAlocar = false;
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

            } while (inputControl.ToUpper() == "P");


            while (_calculaTurs(lstProcesso) > 0)
            {
                foreach (var processo in lstProcesso)
                {
                    Console.WriteLine($"Processo:  {processo.Nome} ");
                    Console.WriteLine($"Tempo restante: {processo.Tur}");
                    Console.WriteLine($"Status: {_showEnumString(processo.Status)}");

                    _down(processo);
                    _up(processo);
                }
                Console.WriteLine($"O processo em execução é o processo: {findByStatus(lstProcesso, EnumStatus.Finalizado).Nome}");
            }
        }
        static int _calculaTurs(List<Processo> lstSemaforo)
        {
            return lstSemaforo.Sum(semaforo => semaforo.Tur);
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
            bool emExecuxao = processo.Status != EnumStatus.EmExecucao;

            if (!emExecuxao)
                Console.WriteLine($"O processo - {processo.Nome} está solicitando um recurso.");

            if (podeAlocar && !emExecuxao)
                processo.Status = EnumStatus.EmExecucao;
            else
            {
                if (processo.Status != EnumStatus.Dormindo && processo.Status != EnumStatus.EmExecucao)
                    processo.Status = EnumStatus.Dormindo;
            }

            podeAlocar = false;
        }

        static Processo findByStatus(List<Processo> lstProcesso, EnumStatus enumStatus)
        {
            return lstProcesso.Where(Processo => Processo.Status == enumStatus).FirstOrDefault();
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
