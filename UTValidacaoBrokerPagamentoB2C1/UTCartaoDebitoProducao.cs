using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using It2b.Zar.Dominio.Entidade;
using It2b.Zar.Dominio.Entidade.Nexxera.CartaoCredito;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UTValidacaoBrokerPagamentoB2C;

namespace UTValidacaoBrokerPagamentoB2C1
{
    [TestClass]
    public class UTCartaoDebitoProducao
    {
        /// <summary>
        /// Atenção para obter sucesso na chamada da transação será necessário um número de cartão válido. Alem da segunda chamada na API, conforme a linha 123 do código de teste.
        /// Caso contrário o resultado será sempre 
        /// </summary>

        #region Propriedades
        private HttpClient ClienteDesenvolvimento;
        private CompraCreditoCartaoCreditoViewModel dadosEnvioCompraCartaoDebitoCapturado;
        private CompraCreditoCartaoCreditoViewModel dadosEnvioCompraCartaoDebitoAutorizado;
        private readonly string ServicoApiProducao = "https://poczarapi.azurewebsites.net/";
        #endregion

        [TestInitialize]
        public void InicializarParametrosTeste()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            if (ClienteDesenvolvimento == null)
            {
                ClienteDesenvolvimento = new HttpClient { BaseAddress = new Uri(ServicoApiProducao) };
                ClienteDesenvolvimento.DefaultRequestHeaders.Accept.Clear();
                ClienteDesenvolvimento.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            // Dados de envio para compra via cartão de débito.
            // Informações do site da nexxera: 11/08/2018           
            dadosEnvioCompraCartaoDebitoCapturado = new CompraCreditoCartaoCreditoViewModel
            {
                AnoValidadeCartao = 2099,
                CodigoSegurancaCartao = "540",
                NomeCartao = "John Captured",
                NumeroBandeira = 0,
                NumeroCartao = "5479743657516940",
                MesValidadeCartao = 12,                
                Valor = 1,
                Usuario = new Usuario { UsuarioId = 33621 }
            };

            //Dados para cartão de crédito errado
            dadosEnvioCompraCartaoDebitoAutorizado = new CompraCreditoCartaoCreditoViewModel
            {
                AnoValidadeCartao = 2099,
                CodigoSegurancaCartao = "111",
                NomeCartao = "John John",
                NumeroBandeira = 0,
                NumeroCartao = "5111962957336921",
                MesValidadeCartao = 12,
                Valor = 1,
                Usuario = new Usuario { UsuarioId = 33621 }
            };
                      

        }

        [TestMethod]
        public void TesteConsumoApiCartaoCreditoNexxeraHomologacao()
        {

            var requestCapturado = new RequestPagamentoCompraCartaoCredito
            {
                AnoValidadeCartao = dadosEnvioCompraCartaoDebitoCapturado.AnoValidadeCartao,
                CodigoSegurancaCartao = dadosEnvioCompraCartaoDebitoCapturado.CodigoSegurancaCartao,
                MesValidadeCartao = dadosEnvioCompraCartaoDebitoCapturado.MesValidadeCartao,
                NomeCartao = dadosEnvioCompraCartaoDebitoCapturado.NomeCartao,
                NomeUsuario = dadosEnvioCompraCartaoDebitoCapturado.NomeCartao,
                NumeroBandeira = dadosEnvioCompraCartaoDebitoCapturado.NumeroBandeira,
                NumeroCartao = dadosEnvioCompraCartaoDebitoCapturado.NumeroCartao,
                Valor = dadosEnvioCompraCartaoDebitoCapturado.Valor,
                ValorPago = dadosEnvioCompraCartaoDebitoCapturado.Valor,
                UsuarioId = dadosEnvioCompraCartaoDebitoCapturado.Usuario.UsuarioId

            };

            var requestAutorizado = new RequestPagamentoCompraCartaoCredito
            {
                AnoValidadeCartao = dadosEnvioCompraCartaoDebitoAutorizado.AnoValidadeCartao,
                CodigoSegurancaCartao = dadosEnvioCompraCartaoDebitoAutorizado.CodigoSegurancaCartao,
                MesValidadeCartao = dadosEnvioCompraCartaoDebitoAutorizado.MesValidadeCartao,
                NomeCartao = dadosEnvioCompraCartaoDebitoAutorizado.NomeCartao,
                NomeUsuario = dadosEnvioCompraCartaoDebitoAutorizado.NomeCartao,
                NumeroBandeira = dadosEnvioCompraCartaoDebitoAutorizado.NumeroBandeira,
                NumeroCartao = dadosEnvioCompraCartaoDebitoAutorizado.NumeroCartao,
                Valor = dadosEnvioCompraCartaoDebitoAutorizado.Valor,
                ValorPago = dadosEnvioCompraCartaoDebitoAutorizado.Valor,
                UsuarioId = dadosEnvioCompraCartaoDebitoAutorizado.Usuario.UsuarioId

            };


            ResponseVendaCartaoCredito retorno;


            //Envio para o gateway de pagamento em ambiente de homologação .  
            //O retorno do tipo 0200 significa transação iniciada.
            using (var response = ClienteDesenvolvimento.PostAsJsonAsync("api/CompraCredito/CartaoDebito", requestCapturado).Result)
            {
                //Se o resposta for válido: 200 
                //Processo iniciado.
                if (response.IsSuccessStatusCode)
                {
                    //Converte a resposta para o objeto ResponseVendaCartaoDebito
                    retorno = response.Content.ReadAsAsync<ResponseVendaCartaoCredito>().Result;
                    requestCapturado.Token = retorno.payment.paymentToken;
                    requestCapturado.DescontoId = 1;

                    Assert.AreEqual("0200", retorno.payment.authorization.returnCode);
                }
            }
            //Este método é o que o Rodrigo disse que estava faltando no desenvolvimento. 
            using (var response = ClienteDesenvolvimento.PostAsJsonAsync($"api/CompraCredito/CartaoDebitoConfirmacao", requestCapturado).Result)
            {
                retorno = response.Content.ReadAsAsync<ResponseVendaCartaoCredito>().Result;
                Assert.AreEqual("0200", retorno.payment.authorization.returnCode);

            }          

        }
    }
}
