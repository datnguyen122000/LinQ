using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinQ
{
    class Program
    {
        static void Main(string[] args)
        {
            var brands = new List<Brand>() {
            new Brand{ID = 1, Name = "Công ty AAA"},
            new Brand{ID = 2, Name = "Công ty BBB"},
            new Brand{ID = 4, Name = "Công ty CCC"},
        };

            var products = new List<Product>()
            {
                new Product(1, "Bàn trà",    400, new string[] {"Xám", "Xanh"},         2),
                new Product(2, "Tranh treo", 400, new string[] {"Vàng", "Xanh"},        1),
                new Product(3, "Đèn trùm",   500, new string[] {"Trắng"},               3),
                new Product(4, "Bàn học",    200, new string[] {"Trắng", "Xanh"},       1),
                new Product(5, "Túi da",     300, new string[] {"Đỏ", "Đen", "Vàng"},   2),
                new Product(6, "Giường ngủ", 500, new string[] {"Trắng"},               2),
                new Product(7, "Tủ áo",      600, new string[] {"Trắng"},               3),
            };
            //lay san pham gia 400    
            var query = from p in products
                        where p.Price == 400
                        select p;
            foreach (var product in query)
            {
                Console.WriteLine(product);
            }
            //lay ten san pham
            Console.WriteLine("=========================Name of Product========================");
            var query1 = products.Select(p => p.Name);
            foreach (var name in query1)
            {
                Console.WriteLine(name);
            }
            //lay ten san pham co chua "tr"
            Console.WriteLine("=========================Product contain 'tr' in name========================");
            var query2 = products.Where(p =>
            {
                return p.Name.Contains("tr");
            });
            foreach (var name in query2)
            {
                Console.WriteLine(name);
            }
            // cac ham min, max, sum, average
            Console.WriteLine("=========================cac ham min, max, sum, average========================");
            Console.WriteLine("=========================Tinh Tong So Chan========================");
            int[] numbers = { 1, 2, 3, 6, 43, 7, 3, 8 };
            Console.WriteLine(numbers.Where(n => n % 2 == 0).Sum());
            // phuong thuc join
            Console.WriteLine("=========================phuong thuc join========================");
            //khong co thi khong de null ma khong lay luon
            var query3 = products.Join(brands, p => p.Brand, b => b.ID, (p, b) =>
            {
                return new
                {
                    Name = p.Name,
                    Brand = b.Name
                };
            });
            foreach (var p in query3)
            {
                Console.WriteLine(p);
            }
            // sap xep tang dan ve gia ORDER BY
            Console.WriteLine("=========================sap xep san pham tang dan theo gia====================");
            products.OrderBy(p => p.Price).ToList().ForEach(products => Console.WriteLine(products));
            Console.WriteLine("=========================sap xep san pham giam dan theo gia====================");
            products.OrderByDescending(p => p.Price).ToList().ForEach(products => Console.WriteLine(products));
            Console.WriteLine("=========================phuong thuc reverse: dao nguoc vi tri phan tu========================");
            //GROUP BY
            var query4 = products.GroupBy(p => p.Price);
            foreach (var product in query4)
            {
                Console.WriteLine("Group by price: " + product.Key);
                foreach (var ỉtem in product)
                {
                    Console.WriteLine(ỉtem);
                }
            }
            //DISTINCT
            Console.WriteLine("=========================Liệt kê các giá trong product không lặp lại các giá giống nhau========================");
            products.Select(p => p.Price).Distinct().ToList().ForEach(price=>Console.WriteLine(price));



            //CÚ PHÁP CỦA LINQ
            /*
                1) Xác định nguồn: from tenphantu in IEnumerables
                   ... where, order by ...
                2) Lấy dữ liệu: select, group by...

             */
            Console.WriteLine("=========================Liệt kê các product gồm tên và giá========================");
            var qr = from a in products
                     select $"{a.Name} : {a.Price}";
            qr.ToList().ForEach(p => Console.WriteLine(p));
            Console.WriteLine("=========================Liệt kê các product gồm tên và giá nhỏ hơn 600 sử dụng kiểu đối tượng vô danh========================");
            var qr1 = from a in products
                     select new
                     {
                         ten=a.Name,
                         gia=a.Price
                     };
            qr1.ToList().ForEach(p => Console.WriteLine(p));

            //gia <=500, màu xanh
            Console.WriteLine("=========================Liệt kê các product gồm giá nhỏ hơn 500 co mau xanh C1========================");
            var qr2 = from p in products
                      where p.Price <= 500 && p.Colors.Contains("Xanh")
                      select new
                      {
                          Ten = p.Name,
                          Gia = p.Price
                      };
            qr2.ToList().ForEach(p => Console.WriteLine(p));
            Console.WriteLine("=========================Liệt kê các product gồm giá nhỏ hơn 500 co mau xanh C2========================");
            var qr3 = from p in products
                      from c in p.Colors
                      where p.Price <= 500 && c==("Xanh")
                      select new
                      {
                          Ten = p.Name,
                          Gia = p.Price
                      };
            qr2.ToList().ForEach(p => Console.WriteLine(p));

            //GROUP BY
            Console.WriteLine("=========================Group By theo giá========================");
            var qr4 = from p in products
                      group p by p.Price into gr
                      let sl = "So luong la " + gr.Count()
                      select new
                      {
                          ten = gr.Key,
                          cacsanpham = gr.ToList(),
                          soluong = sl

                      };
            qr4.ToList().ForEach(group =>
            {
                Console.WriteLine(group.ten);
                Console.WriteLine(group.soluong);
                group.cacsanpham.ForEach(p => Console.WriteLine(p));
            });


            //JOIN TRONG SQL
            Console.WriteLine("=========================Liệt kê sản phẩm theo kèm tên thương hiệu của sản phẩm đó========================");
            var qr5 = from p in products
                      join b in brands on p.Brand equals b.ID into t 
                      from b in t.DefaultIfEmpty()// để vẫn lấy được product nếu nó không có brand trong bang brand
                      select new
                      {
                          ten = p.Name,
                          gia = p.Price,
                          thuonghieu = (b!=null)?b.Name:"Không có thương hiệu"
                      };
            qr5.ToList().ForEach(o =>
            {
                Console.WriteLine($"{o.ten,10}{o.thuonghieu,30}{o.gia,5}");
            });
        }


    }
}
