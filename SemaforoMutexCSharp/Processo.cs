namespace SemaforoMutexCSharp
{
    class Processo
    {
        public Processo(string pNome, int pTur)
        {
            Nome = pNome;
            Tur = pTur;
            Status = EnumStatus.Livre;
        }
        public string Nome { get; set; }
        public int Tur { get; set; }
        public EnumStatus Status { get; set; }


        public void DecrementaTur()
        {
            Tur--;
        }

    }
}
