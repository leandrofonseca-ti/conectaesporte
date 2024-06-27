using System.ComponentModel.DataAnnotations.Schema;

namespace ConectaEsporte.Web.Models
{
    public class PedidoModel
    {
        public DateTime Data { get; set; }
        public int TipoTransacao { get; set; }
        public string TransacaoID { get; set; }
        public string Referencia { get; set; }
        public int Status { get; set; }

        [NotMapped]
        public string StatusDesc
        {
            get
            {
                if (Status == 1)
                    return "Aguardando pagamento";
                else if (Status == 2)
                    return "Em análise";
                else if (Status == 3)
                    return "Paga";
                else if (Status == 4)
                    return "Disponível";
                else if (Status == 5)
                    return "Em disputa";
                else if (Status == 6)
                    return "Devolvida";
                else if (Status == 7)
                    return "Cancelada";
                else
                    return "Status Desconhecido";
            }
        }
        public decimal Total { get; set; }
        public decimal TotalDescontos { get; set; }
        public decimal TotalTaxas { get; set; }
        public decimal TotalLiquido { get; set; }
        public DateTime DataLiberacaoCredito { get; set; }
        public int TotalItensPedido { get; set; }

        public PagamentoModel Pagamento = new PagamentoModel();

        public ClienteModel Cliente = new ClienteModel();

        public List<ItemModel> Itens = new List<ItemModel>();

    }



    public class ClienteModel
    {
        public int IDCliente { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string TelefoneCodigoArea { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }
        public string EnderecoNumero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Pais { get; set; }
        public string CEP { get; set; }
    }

    public class PagamentoModel
    {
        public int Tipo { get; set; }
        public int Parcelas { get; set; }
        public string TipoDesc
        {
            get
            {
                if (Tipo == 1)
                    return "Cartão de crédito";
                else if (Tipo == 2)
                    return "Boleto";
                else if (Tipo == 3)
                    return "Débito online (TEF)";
                else if (Tipo == 4)
                    return "Saldo PagSeguro";
                else if (Tipo == 5)
                    return "Oi Paggo";
                else if (Tipo == 7)
                    return "Depósito em conta";
                else
                    return "Origem Desconhecida";
            }
        }
        public int Codigo { get; set; }
        public string CodigoDesc
        {
            get
            {
                if (Codigo == 101)
                    return "Cartão de crédito Visa";
                else if (Codigo == 102)
                    return "Cartão de crédito MasterCard";
                else if (Codigo == 103)
                    return "Cartão de crédito American Express";
                else if (Codigo == 104)
                    return "Cartão de crédito Diners.";
                else if (Codigo == 105)
                    return "Cartão de crédito Hipercard";
                else if (Codigo == 106)
                    return "Cartão de crédito Aura";
                else if (Codigo == 107)
                    return "Cartão de crédito Elo";
                else if (Codigo == 108)
                    return "Cartão de crédito PLENOCard";
                else if (Codigo == 109)
                    return "Cartão de crédito PersonalCard";
                else if (Codigo == 110)
                    return "Cartão de crédito JCB";
                else if (Codigo == 111)
                    return "Cartão de crédito Discover";
                else if (Codigo == 112)
                    return "Cartão de crédito BrasilCard";
                else if (Codigo == 113)
                    return "Cartão de crédito FORTBRASIL";
                else if (Codigo == 114)
                    return "Cartão de crédito CARDBAN";
                else if (Codigo == 115)
                    return "Cartão de crédito VALECARD";
                else if (Codigo == 116)
                    return "Cartão de crédito Cabal";
                else if (Codigo == 117)
                    return "Cartão de crédito Mais";
                else if (Codigo == 118)
                    return "Cartão de crédito Avista";
                else if (Codigo == 119)
                    return "Cartão de crédito GRANDCARD";
                else if (Codigo == 120)
                    return "Cartão de crédito Sorocred";
                else if (Codigo == 201)
                    return "Boleto Bradesco";
                else if (Codigo == 202)
                    return "Boleto Santander";
                else if (Codigo == 301)
                    return "Débito online Bradesco";
                else if (Codigo == 302)
                    return "Débito online Itaú";
                else if (Codigo == 303)
                    return "Débito online Unibanco";
                else if (Codigo == 304)
                    return "Débito online Banco do Brasil";
                else if (Codigo == 305)
                    return "Débito online Banco Real";
                else if (Codigo == 306)
                    return "Débito online Banrisul";
                else if (Codigo == 307)
                    return "Débito online HSBC";
                else if (Codigo == 401)
                    return "Saldo PagSeguro";
                else if (Codigo == 501)
                    return "Oi Paggo";
                else if (Codigo == 701)
                    return "Depósito em conta - Banco do Brasil";
                else if (Codigo == 702)
                    return "Depósito em conta - HSBC";
                else
                    return "Origem Desconhecida";
            }
        }
        public string URL { get; set; }
    }

    public class ItemModel
    {
        public int ID { get; set; }
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal Preco { get; set; }
    }
}
