using System;

/*Class Invariants:
  This class was created as a representative of an Employee that is also a Customer. It works using Multiple inheritance in a way that allows the client to
  use this class in both heterogeneous colloections of Employee (by using the interface) and Customer. In addition to this, it can be a segmentCustomer or a 
  pickyCustomer, depending on what the client desires. much of the implementation calls the other customer functions polymorphically and in general 
  reuses most of the code. One thing unique to this class is that reducing the balance in order to pay for something will first take money out of the employees
  upcoming salary before resorting to depleting their balance.
*/

//Interface Invariants:
// constructor
//  -This is how you create a employeeCustomer object with all the data the it will need to function
//  -by default the pay level will be the lowest one and the default customer will be the base one
// paySalary
//  -adds salary to current balance depending on the employees pay level
// setPayLevel
//  -allows for the client to change the employees pay level to accomodate things like demotions and promotions
// getPayAmnt
//  -getter method that allows to check the employees current salary
// changeBalance
//  -will subtract from salary yet to be paid before subtracting from balance
// addClassification & addExclusion
//  -will only work if EmployeeCustomer was initialized to be the appropriate type. if it does not work then will return false.
//  -otherwise will work exactly the same as respective functions in SegmentCustomer and PickyCustomer
// checkItem
//  -will return if item is valid based on the current setup and accomodations of the object



namespace P3
{
    public class EmployeeCustomer : Customer, Iemployee
    {
        private Customer customer;
        private Employee employee;
        private bool isPickyCustomer = false;
        private bool isSegCustomer = false;
        private double toBePaid;

        //Preconditions: data desired to fill object is inputed
        //Postconditions: all fields are filled out based on inputed information
        public EmployeeCustomer(string Name = "", string Address = "", double Balance = 0, double MinimumPrice = 0, int classType = 0, int payLvl = 1, string[] dependancy = null)
        {
            if (classType == 1)
            {
                customer = new PickyCustomer(Name, Address, Balance, MinimumPrice, dependancy);
                isPickyCustomer = true;
            }
            else if (classType == 2)
            {
                customer = new SegmentCustomer(Name, Address, Balance, MinimumPrice, dependancy);
                isSegCustomer = true;
            }
            else
            {
                customer = new Customer(Name, Address, Balance, MinimumPrice);
            }
            employee = new Employee(payLvl, Balance);
            toBePaid = employee.getPayAmnt();
        }

        //Preconditions: produce is inputed
        //Postconditions: none
        public override bool checkItem(Produce p)
        {
            return customer.checkItem(p);
        }

        //Preconditions: None
        //Postconditions: balance is increased by salary amount minus any purchase costs
        public void paySalary()
        {
            base.changeBalance(toBePaid);
            toBePaid = employee.getPayAmnt();
        }

        //Preconditions: double of how balance should change is inputed is inputed
        //Postconditions: if negative, will subtract from next salary before balance. balance and salary will not go negative
        public override void changeBalance(double change)
        {
            if (change < 0)
            {
                toBePaid += change;
                if (toBePaid < 0)
                {
                    base.changeBalance(toBePaid);
                    toBePaid = 0;
                }
            }
            else
            {
                base.changeBalance(change);
            }
        }

        //Preconditions: string must be inputed
        //Postconditions: if SegmentCustomer, string is added to includeClassifications, if it is not big enough, its size is increased
        public bool addClassification(string clasif)
        {
            if (isSegCustomer)
            {
                SegmentCustomer sc = (SegmentCustomer)customer;
                sc.addClassification(clasif);
                return true;
            }
            return false;
        }

        //Preconditions: string must be inputed
        //Postconditions: if PickyCustomer, exclusion is added to list(and replaces oldest value if there are 5 already)
        public bool addExclusion(string exc)
        {
            if (isPickyCustomer)
            {
                PickyCustomer pc = (PickyCustomer)customer;
                pc.addExclusion(exc);
                return true;
            }
            return false;
        }

        //Preconditions: new payLevel is inputed
        //Postconditions: payLevel is set to inputed one
        public void setPayLvl(int payLevel)
        {
            employee.setPayLvl(payLevel);
        }

        //Preconditions: None
        //Postconditions: none
        public double getPayAmnt()
        {
            return toBePaid;
        }
    }
}

/*Implementation Invariants
  Public Functions:
  Constructer
  - the constructor has default values  for error checking, all code will work with default values
  - fields are set to inputed equivilants and pay values are set
  - uses double composition for the implementation in order to reuse a lot of code polymorphically
  - Customer is initialized to hold a different type depending on what is inputed
  paySalary
  -adds salary to current balance using to be paid variable to account for salary deductions
  setPayLevel
  -simple setter with some error checking
  getPayAmnt
  -getter method 
  addClassification & addExclusion
  - checks if typing is correct before executing respective functions via casting
  checkItem
  - because of composition, will use check item dynamically using vtab
  changeBalance
  - uses tobepaid variable to subtract from salaray before subtracting from balance
*/