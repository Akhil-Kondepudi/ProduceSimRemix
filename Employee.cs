using System;

/*Class Invariants:
  This class was created as a representative of an Employee. It gives the employee a salary that can be paid out using a method, and the 
  salary is dependent on the employees pay level. The higher the pay level the higher the pay, with the current being pay level 3 with a 
  weekly salary of 1000$. It makes use of an employee interface that enables this to be included in a heterogeneous collection with the 
  class EmployeeCustomer.
*/

//Interface Invariants:
// constructor
//  -This is how you create a employee object with all the data the it will need to function
//  -by default the pay level will be the lowest one and the initial balance will be 0
// paySalary
//  -adds salary to current balance depending on the employees pay level
// setPayLevel
//  -allows for the client to change the employees pay level to accomodate things like demotions and promotions
// getPayAmnt
//  -getter method that allows to check the employees current salary


namespace P3
{
    public interface Iemployee
    {
        void paySalary();
        void setPayLvl(int payLevel);
        double getPayAmnt();

    }
    public class Employee : Iemployee
    {
        protected double employeeBalance;
        protected int payLvl;
        protected double[] PayAmnt;

        //Preconditions: data desired to fill object is inputed
        //Postconditions: all fields are filled out based on inputed information
        public Employee(int payLevel = 1, double initialBalance = 0.0)
        {
            employeeBalance = initialBalance < 0 ? 0 : initialBalance;
            payLvl = (payLevel - 1) % 3;
            payLvl = payLvl < 0 ? 0 : payLvl;
            PayAmnt = new double[3];
            PayAmnt[0] = 100.0;
            PayAmnt[1] = 500.0;
            PayAmnt[2] = 1000.0;
        }

        //Preconditions: none
        //Postconditions: balance is increased based on salary
        public void paySalary()
        {
            employeeBalance += PayAmnt[payLvl];
        }

        //Preconditions: new pay level is inputed
        //Postconditions: pay level is changed based on inputed one.
        public void setPayLvl(int payLevel)
        {
            payLvl = (payLevel - 1) % 3;
            payLvl = payLvl < 0 ? 0 : payLvl;
        }

        //Preconditions: None
        //Postconditions: None
        public double getPayAmnt()
        {
            return PayAmnt[payLvl];
        }
    }
}

/*Implementation Invariants
  Public Functions:
  Constructer
  - the constructor has default values  for error checking, all code will work with default values
  - fields are set to inputed equivilants and pay values are set
  paySalary
  -adds salary to current balance depending on the employees pay level
 setPayLevel
 -simple setter with some error checking
 getPayAmnt
 -getter method 
*/