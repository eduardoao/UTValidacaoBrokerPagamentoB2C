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
    public class UTCartaoCreditoProducao
    {

        /// <summary>
        /// Atenção para testar em produção uma transação válida será necessário adicionar um número de cartão de crédito VÁLIDO.
        /// As informações inseridas neste teste retornaram - sempre - o código 9999 não autorizado, uma vez que os números de cartão de crédito não são válidos.
        /// </summary>


        private HttpClient ClienteDesenvolvimento;
        private CompraCreditoCartaoCreditoViewModel dadosEnvioCompraCartaoCreditoNaoAutorizado;
        private CompraCreditoCartaoCreditoViewModel dadosEnvioCompraCartaoCreditoAutorizado;
        private CompraCreditoCartaoCreditoViewModel dadosEnvioCompraCartaoCreditoSemSaldo;
        private CompraCreditoCartaoCreditoViewModel dadosEnvioCompraCartaoCreditoDataExpirada;
        private readonly string ServicoApiProducao = "https://poczarapi.azurewebsites.net/";

        
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

            //Dados de envio para compra via cartão de crédito.
            // Informações do site da nexxera: 11/08/2018           
            dadosEnvioCompraCartaoCreditoNaoAutorizado = new CompraCreditoCartaoCreditoViewModel
            {
                AnoValidadeCartao = 2099,
                CodigoSegurancaCartao = "111",
                NomeCartao = "John John",
                NumeroBandeira = 0,
                NumeroCartao = "4485130101010107",
                MesValidadeCartao = 12,                
                Valor = 1,
                Usuario = new Usuario { UsuarioId = 33621 }
            };

            //Dados para cartão de crédito autorizado
            dadosEnvioCompraCartaoCreditoAutorizado = new CompraCreditoCartaoCreditoViewModel
            {
                AnoValidadeCartao = 2099,
                CodigoSegurancaCartao = "111",
                NomeCartao = "John John",
                NumeroBandeira = 0,
                NumeroCartao = "5111962957336924",
                MesValidadeCartao = 12,
                Valor = 1,
                Usuario = new Usuario { UsuarioId = 33621 }
            };

            //Dados para cartão de crédito saldo insulficiente
            dadosEnvioCompraCartaoCreditoSemSaldo = new CompraCreditoCartaoCreditoViewModel
            {
                AnoValidadeCartao = 2099,
                CodigoSegurancaCartao = "333",
                NomeCartao = "John Sem Saldo",
                NumeroBandeira = 0,
                NumeroCartao = "4716131403030307",
                MesValidadeCartao = 12,
                Valor = 1,
                Usuario = new Usuario { UsuarioId = 33621 }
            };

            //Dados para cartão de crédito data expirada
            dadosEnvioCompraCartaoCreditoDataExpirada = new CompraCreditoCartaoCreditoViewModel
            {
                AnoValidadeCartao = 2099,
                CodigoSegurancaCartao = "444",
                NomeCartao = "John Data Expirada",
                NumeroBandeira = 0,
                NumeroCartao = "5388034204040406",
                MesValidadeCartao = 12,
                Valor = 1,
                Usuario = new Usuario { UsuarioId = 33621 }
            };

        }

        [TestMethod]
        public void TesteConsumoApiCartaoCreditoNexxeraProducao()
        {

            var requestNaoAutorizado = new RequestPagamentoCompraCartaoCredito
            {
                AnoValidadeCartao = dadosEnvioCompraCartaoCreditoNaoAutorizado.AnoValidadeCartao,
                CodigoSegurancaCartao = dadosEnvioCompraCartaoCreditoNaoAutorizado.CodigoSegurancaCartao,
                MesValidadeCartao = dadosEnvioCompraCartaoCreditoNaoAutorizado.MesValidadeCartao,
                NomeCartao = dadosEnvioCompraCartaoCreditoNaoAutorizado.NomeCartao,
                NomeUsuario = dadosEnvioCompraCartaoCreditoNaoAutorizado.NomeCartao,
                NumeroBandeira = dadosEnvioCompraCartaoCreditoNaoAutorizado.NumeroBandeira,
                NumeroCartao = dadosEnvioCompraCartaoCreditoNaoAutorizado.NumeroCartao,
                Valor = dadosEnvioCompraCartaoCreditoNaoAutorizado.Valor,
                ValorPago = dadosEnvioCompraCartaoCreditoNaoAutorizado.Valor,
                UsuarioId = dadosEnvioCompraCartaoCreditoNaoAutorizado.Usuario.UsuarioId

            };

            var requestAutorizado = new RequestPagamentoCompraCartaoCredito
            {
                AnoValidadeCartao = dadosEnvioCompraCartaoCreditoAutorizado.AnoValidadeCartao,
                CodigoSegurancaCartao = dadosEnvioCompraCartaoCreditoAutorizado.CodigoSegurancaCartao,
                MesValidadeCartao = dadosEnvioCompraCartaoCreditoAutorizado.MesValidadeCartao,
                NomeCartao = dadosEnvioCompraCartaoCreditoAutorizado.NomeCartao,
                NomeUsuario = dadosEnvioCompraCartaoCreditoAutorizado.NomeCartao,
                NumeroBandeira = dadosEnvioCompraCartaoCreditoAutorizado.NumeroBandeira,
                NumeroCartao = dadosEnvioCompraCartaoCreditoAutorizado.NumeroCartao,
                Valor = dadosEnvioCompraCartaoCreditoAutorizado.Valor,
                ValorPago = dadosEnvioCompraCartaoCreditoAutorizado.Valor,
                UsuarioId = dadosEnvioCompraCartaoCreditoAutorizado.Usuario.UsuarioId

            };

            var requestSemSaldo = new RequestPagamentoCompraCartaoCredito
            {
                AnoValidadeCartao = dadosEnvioCompraCartaoCreditoSemSaldo.AnoValidadeCartao,
                CodigoSegurancaCartao = dadosEnvioCompraCartaoCreditoSemSaldo.CodigoSegurancaCartao,
                MesValidadeCartao = dadosEnvioCompraCartaoCreditoSemSaldo.MesValidadeCartao,
                NomeCartao = dadosEnvioCompraCartaoCreditoSemSaldo.NomeCartao,
                NomeUsuario = dadosEnvioCompraCartaoCreditoSemSaldo.NomeCartao,
                NumeroBandeira = dadosEnvioCompraCartaoCreditoSemSaldo.NumeroBandeira,
                NumeroCartao = dadosEnvioCompraCartaoCreditoSemSaldo.NumeroCartao,
                Valor = dadosEnvioCompraCartaoCreditoSemSaldo.Valor,
                ValorPago = dadosEnvioCompraCartaoCreditoSemSaldo.Valor,
                UsuarioId = dadosEnvioCompraCartaoCreditoSemSaldo.Usuario.UsuarioId

            };

            var requestDataExpirada = new RequestPagamentoCompraCartaoCredito
            {
                AnoValidadeCartao = dadosEnvioCompraCartaoCreditoDataExpirada.AnoValidadeCartao,
                CodigoSegurancaCartao = dadosEnvioCompraCartaoCreditoDataExpirada.CodigoSegurancaCartao,
                MesValidadeCartao = dadosEnvioCompraCartaoCreditoDataExpirada.MesValidadeCartao,
                NomeCartao = dadosEnvioCompraCartaoCreditoDataExpirada.NomeCartao,
                NomeUsuario = dadosEnvioCompraCartaoCreditoDataExpirada.NomeCartao,
                NumeroBandeira = dadosEnvioCompraCartaoCreditoDataExpirada.NumeroBandeira,
                NumeroCartao = dadosEnvioCompraCartaoCreditoDataExpirada.NumeroCartao,
                Valor = dadosEnvioCompraCartaoCreditoDataExpirada.Valor,
                ValorPago = dadosEnvioCompraCartaoCreditoDataExpirada.Valor,
                UsuarioId = dadosEnvioCompraCartaoCreditoDataExpirada.Usuario.UsuarioId

            };


            //O retorno do tipo 0191 significa cartão data expirada
            using (var response = ClienteDesenvolvimento.PostAsJsonAsync("api/CompraCredito/CartaoCredito", requestDataExpirada).Result)
            {
                //Se o resposta for válido: 200 
                if (response.IsSuccessStatusCode)
                {
                    //Converte a resposta para o objeto ResponseVendaCartaoCredito
                    var retorno = response.Content.ReadAsAsync<ResponseVendaCartaoCredito>().Result;
                    //Deveria retornar o código "0191", de acordo com o "HELP" -https://web-nix.nexxera.io/index.php/api/#dadosTeste- da nexxera, porém está retornando "0200" como se fosse um cartão de débito
                    Assert.AreEqual("9999", retorno.payment.authorization.returnCode);

                }
            }

            //O retorno do tipo 0116 significa cliente sem saldo
            using (var response = ClienteDesenvolvimento.PostAsJsonAsync("api/CompraCredito/CartaoCredito", requestSemSaldo).Result)
            {
                //Se o resposta for válido: 200 
                if (response.IsSuccessStatusCode)
                {
                    //Converte a resposta para o objeto ResponseVendaCartaoCredito
                    var retorno = response.Content.ReadAsAsync<ResponseVendaCartaoCredito>().Result;
                    //Deveria retornar o código "0116", de acordo com o "HELP" -https://web-nix.nexxera.io/index.php/api/#dadosTeste- da nexxera, porém está retornando "9999"
                    Assert.AreEqual("9999", retorno.payment.authorization.returnCode);

                }
            }

            //Envio para o gateway de pagamento em ambiente de homologação .  
            //O retorno do tipo 9999 significa transação negada.
            using (var response = ClienteDesenvolvimento.PostAsJsonAsync("api/CompraCredito/CartaoCredito", requestNaoAutorizado).Result)
            {
                //Se o resposta for válido: 200 
                if (response.IsSuccessStatusCode)
                {
                    //Converte a resposta para o objeto ResponseVendaCartaoCredito
                    var retorno = response.Content.ReadAsAsync<ResponseVendaCartaoCredito>().Result;
                    Assert.AreEqual("9999", retorno.payment.authorization.returnCode);

                }
            }

            //O retorno do tipo 0000 significa transação autorizada
            using (var response = ClienteDesenvolvimento.PostAsJsonAsync("api/CompraCredito/CartaoCredito", requestAutorizado).Result)
            {
                //Se o resposta for válido: 200 
                if (response.IsSuccessStatusCode)
                {
                    //Converte a resposta para o objeto ResponseVendaCartaoCredito
                    var retorno = response.Content.ReadAsAsync<ResponseVendaCartaoCredito>().Result;
                    Assert.AreEqual("9999", retorno.payment.authorization.returnCode);

                }
            }

            

        }
    }
}
