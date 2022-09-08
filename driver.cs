//Author: Akhil Kondepudi
//Date: 1/12/2022
//Revision History: Creation finished on 1/12/2022 by Akhil Kondepudi
using System;

namespace P3
{
    public class Driver
    {
        const string filename = "ProduceListTabDelimited-1.txt";
        static string[] names = { "Celery Root", "Parsely", "Pear", "Cilantro", "Orange", "Apple", "Crimini", "Potato", "Sage", "Spinach", "Kiwi" };
        const int namesSize = 11;
        static string[] classific = { "fruit", "vegetable", "fungi", "herb" };
        const int classificSize = 4;
        const int listMaxSize = 40;
        const int numClass = 4;
        const int maxPaylvl = 3;
        static Random rand = new Random();
        public static void Main(string[] args)
        {
            welcomeMessage();
            List<Customer> custList = new List<Customer>();
            List<Delivery> deliList = new List<Delivery>();
            List<Iemployee> employeeList = new List<Iemployee>();
            fillLists(custList, deliList, employeeList);
            employeeTest(employeeList);
            theWorks(deliList);

            goodbyeMessage();

        }

        public static void welcomeMessage()
        {
            Console.WriteLine("Welcome to the Driver Demo");
        }

        public static void fillLists(List<Customer> custList, List<Delivery> deliList, List<Iemployee> employeeList)
        {
            int upper = rand.Next(0, listMaxSize);
            int num;
            for (int i = 0; i < upper; i++)
            {
                num = rand.Next(0, numClass);
                if (num % numClass == 0)
                {
                    custList.Add(randCustomer());
                }
                else if (num % numClass == 1)
                {
                    custList.Add(randPickyCustomer());
                }
                else if (num % numClass == 2)
                {
                    EmployeeCustomer temp = randEmployeeCustomer();
                    custList.Add(temp);
                    employeeList.Add(temp);
                }
                else
                {
                    custList.Add(randSegmentCustomer());
                }
            }
            upper = rand.Next(0, listMaxSize) / 2;
            for (int i = 0; i < upper; i++)
            {
                employeeList.Add(randEmployee());
            }
            foreach (Customer cust in custList)
            {
                deliList.Add(new Delivery(cust, filename));
            }

        }

        public static Customer randCustomer()
        {
            return new Customer("", "", rand.Next(100, 10000) / 100.00, rand.Next(100, 10000) / 100.00);
        }

        public static PickyCustomer randPickyCustomer()
        {
            PickyCustomer pc = new PickyCustomer("", "", rand.Next(100, 10000) / 100.00, rand.Next(100, 10000) / 100.00);
            int upper = rand.Next(0, 10);
            for (int x = 0; x < upper; x++)
            {
                pc.addExclusion(names[rand.Next(0, namesSize)]);
            }
            return pc;
        }
        public static SegmentCustomer randSegmentCustomer()
        {
            SegmentCustomer sc = new SegmentCustomer("", "", rand.Next(100, 10000) / 100.00, rand.Next(100, 10000) / 100.00);
            int upper = rand.Next(0, 4);
            for (int i = 0; i < upper; i++)
            {
                sc.addClassification(classific[rand.Next(0, classificSize)]);
            }
            return sc;
        }

        public static EmployeeCustomer randEmployeeCustomer()
        {
            EmployeeCustomer ec = new EmployeeCustomer("", "", rand.Next(100, 10000) / 100.00, rand.Next(100, 10000) / 100.00, rand.Next(0, numClass), rand.Next(0, maxPaylvl));
            int upper = rand.Next(0, 4);
            for (int i = 0; i < upper; i++)
            {
                ec.addClassification(classific[rand.Next(0, classificSize)]);
            }
            upper = rand.Next(0, 10);
            for (int x = 0; x < upper; x++)
            {
                ec.addExclusion(names[rand.Next(0, namesSize)]);
            }
            return ec;
        }

        public static Employee randEmployee()
        {
            Employee e = new Employee(rand.Next(0, maxPaylvl), rand.Next(100, 10000) / 100.00);
            
            return e;
        }

        public static void employeeTest(List<Iemployee> employeeList)
        {
            foreach (Iemployee e in employeeList)
            {
                e.paySalary();
            }
        }

        public static void theWorks(List<Delivery> deliList)
        {
            foreach (Delivery deli in deliList)
            {
                int size = rand.Next(0, namesSize);
                string[] wanted = new string[size];
                for (int i = 0; i < size; i++)
                {
                    wanted[i] = names[rand.Next(0, namesSize)];
                }
                deli.ForcastDelivery(wanted);
                deli.FillBox();
                deli.DeliverBox();
            }
        }

        public static void goodbyeMessage()
        {
            Console.WriteLine("The program has ended.");
        }
    }
}