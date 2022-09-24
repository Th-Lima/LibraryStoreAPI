﻿using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace LibraryStore.Api.Dtos
{
    public class ProviderDto
    {
        [Key]
        public Guid Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 2)]
        public string Name { get; set; }

        [Display(Name = "Documento")]
        [Required(ErrorMessage = "O Campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        public string Document { get; set; }

        [Display(Name = "Tipo")]
        public int TypeProvider { get; set; }

        [Display(Name = "Endereço")]
        public AddressDto Address { get; set; }

        [Display(Name = "Ativo")]
        public bool Active { get; set; }

        //[Display(Name = "Produtos")]
        //public IEnumerable<ProductDto> Products { get; set; }
    }
}
