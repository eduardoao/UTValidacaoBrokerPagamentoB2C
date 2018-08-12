using It2b.Zar.Dominio.Entidade;
using It2b.Zar.Dominio.Entidade.Nexxera.CartaoCredito;

namespace UTValidacaoBrokerPagamentoB2C
{
    public class CompraCreditoCartaoCreditoViewModel
    {
        public int NumeroBandeira { get; set; }
        public string NumeroCartao { get; set; }
        public string NomeCartao { get; set; }
        public int MesValidadeCartao { get; set; }
        public int AnoValidadeCartao { get; set; }
        public string CodigoSegurancaCartao { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPago { get; set; }
        public int? DescontoId { get; set; }
        public int Parcelas { get; set; } 
        public ResponseVendaCartaoCredito ResponseCartaoCredito { get; set; }
        public Usuario Usuario { get; set; }
    }
}
