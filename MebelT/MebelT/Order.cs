//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MebelT
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Order
    {
       /* [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            this.Order_id = 1;
        }*/
        [Required(ErrorMessage="Заполните поле")]
        public int Order_id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public Nullable<System.DateTime> Order_date { get; set; }
        public Nullable<int> Mebel_id { get; set; }
        public Nullable<int> Material_id { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public Nullable<System.DateTime> Completion_date { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public int Price_for_one { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public int Amount { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public int Total_price { get; set; }
        [Required(ErrorMessage = "Заполните поле")]
        public string Client { get; set; }
    
        public virtual Material Material { get; set; }
        public virtual Mebel Mebel { get; set; }
    }
}
