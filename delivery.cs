//Author: Akhil Kondepudi
//Date: 1/13/2022
//Revision History: Creation finished on 1/13/2022 by Akhil Kondepudi
using System;

/*Class Invariants:
  This class was created as a way for a delivery service to keep track of the current deliveries.
  It allows for current storage availabel for delivery to be updated and based and that and what items
  that the customer wants (which is gotten from ForcastDelivery) can fill up the delivery up to the customers
  minimum spending amount. The object also supports deep copying incase there are any duplicate orders that the
  customre wants to happen again. Items can be replaced based on the item name. If the amount in the delivery box actually
  exceeds the amount of balance that the customer has, then the extra will be given to the customer for free, as it is impossible to
  know if it is possible to be above the miminmum Price but below the balance with the costs of all the produce items.
*/

//Interface Invariants:
// Delivery constructor
//  -This is how you create a delivery object with all the data the it will need to function
//  -This will only work if the Preconditions are followef explicitly
// Delivery destructor
//  -destroys the delivery object
// Delivery copy constructor
//  -Creates a delivery object with the exact same data as the inputed delivery object
//  -Only works if a valid delivery object is inputed
// Overoaded assignment operator
//  -Creates a delivery object with the exact same data as the inputed delivery object
//  -Only works if a valid delivery object is inputed
// ReplaceItem
//  -takes in the name of the produce item you want to replace and replaces it with the inputed produce item
//  -will return false and not perform replacement if unable to find name or inputed produce item expired or spoiled
// DeliverBox
//  -Will deliver all the items currently stored in the class to the client, will replace any spoiled or expired
//   produce items with anything that the object has in storage that is not expired
//  -will also reduce the clients balance by the amount that the delivery cost if the delivery costs more than the clients balance
//   then it will give the extra stuff to the client for free
// ShareOrder
//  -will share info of each produce item set to be delivered in the form of a string array
// FillBox
//  -will fill the box to be delivered with items in storage based on what the forcast delivery was/wanted
//  -Also takes in a vector of Produce items that the client wants to add to storage if desired
// ForcastDelivery
//  -Takes in a vector of strings that is the name of the produce items that the client wants added to the delivery

namespace P3
{
    public class Delivery
    {
        private double totalCost = 0;
        private Customer customer;
        private Produce[] produceList = { };
        private int prodListSize;
        private Produce[] deliveryBox = { };
        private int deliveryBoxListSize = 0;
        private string[] wantedItems = { };
        private int wantedItemsSize = 0;

        // Preconditions: Valid customer object and array containing storage of produce items must be inputed
        // Postconditions: Delivery object is instianitated holding inputed data
        public Delivery(Customer c, Produce[] initialList)
        {
            customer = c;
            prodListSize = initialList.Length;
            produceList = new Produce[prodListSize];
            for (int i = 0; i < initialList.Length; i++)
            {
                produceList[i] = new Produce(initialList[i]);
            }
        }

        // Preconditions: Valid customer object and filename of file holding produce information in specific format inputed
        // Postconditions: Delivery object is instianitated holding inputed data
        public Delivery(Customer c, string filename) 
        {
            customer = c;
            prodListSize = 0;
            produceList = new Produce[5];
            string[] lines = File.ReadAllLines("..\\..\\..\\" + filename); //relative path for visual studio probably different on different ide
            string[] item;
            string[] date;
            string harvdate;
            string expiredate;
            int cnt;
            double cost;
            for (int i = 1; i < lines.Length; i++)
            {
                item = lines[i].Split('\t');
                date = item[6].Split('/');
                harvdate = date[0] + "-" + date[1] + "-" + date[2] + "-1200";
                expiredate = date[0] + "-" + (Int32.Parse(date[1]) + 7) + "-" + date[2] + "-1200";
                if (Double.Parse(item[1]) < 1)
                {
                    cnt = 1;
                    cost = (1 / Double.Parse(item[1])) * Double.Parse(item[3]);
                }
                else
                {
                    cnt = (int)Math.Round(Double.Parse(item[1]));
                    cost = Double.Parse(item[3]);
                }

                if (prodListSize == produceList.Length)
                {
                    Array.Resize(ref produceList, produceList.Length + 5);
                }
                produceList[prodListSize] = new Produce(item[0], cnt, cost, item[2], item[4], item[5], harvdate, item[5], expiredate);
                prodListSize++;
            }
        }

        // Preconditions: Valid Produce object inputed
        // Postconditions: returns true if produce is usable
        private bool checkValid(Produce p, string testTime = "")
        {
            bool val = !(p.checkSpoiled(testTime) || p.checkExpired(testTime));
            return val;
        }

        // Preconditions: name of produce item and produce item must be inputed
        // Postconditions: produce item with name in the delivery box is replaced with inputed produce object. if replacing failed return false;
        public bool ReplaceItem(string itemName, Produce replaced, string testTime = "")
        {
            int i = 0;
            while (i < deliveryBoxListSize && checkValid(replaced, testTime))
            {
                if (deliveryBox[i].info()[0] == itemName)
                {
                    totalCost -= Int32.Parse(deliveryBox[i].info()[1]) * Double.Parse(deliveryBox[i].info()[3]);
                    totalCost += Int32.Parse(replaced.info()[1]) * Double.Parse(replaced.info()[3]);
                    deliveryBox[i] = replaced;
                    return true;
                }
                i++;
            }
            return false;
        }

        // Preconditions: None
        // Postconditions: array containg information about current items in delivery box are transfered to customer if valid
        //                 else they items are replaced and other items fill box until minimum price has been passed
        public void DeliverBox(string testTime = "")
        {
            bool replaced;
            for (int i = 0; i < deliveryBoxListSize; i++)
            {
                if (!checkValid(deliveryBox[i], testTime) || !customer.checkItem(deliveryBox[i]))
                {
                    replaced = false;
                    for (int x = 0; x < prodListSize; x++)
                    {
                        if (checkValid(produceList[x], testTime) && customer.checkItem(produceList[x]))
                        {
                            ReplaceItem(deliveryBox[i].info()[0], produceList[x]);

                            removeVal(produceList, x, true);

                            replaced = true;
                            x = prodListSize;
                        }
                    }
                    if (!replaced)
                    {
                        totalCost -= Int32.Parse(deliveryBox[i].info()[1]) * Double.Parse(deliveryBox[i].info()[3]);
                        removeVal(deliveryBox, i, false);
                    }
                    i--;
                }
            }

            bool found;
            while (totalCost < Double.Parse(customer.info()[3]))
            {
                found = false;
                for (int x = 0; x < prodListSize; x++)
                {
                    if (checkValid(produceList[x], testTime) && customer.checkItem(produceList[x]))
                    {
                        found = true;
                        if (deliveryBoxListSize == deliveryBox.Length)
                        {
                            Array.Resize(ref deliveryBox, deliveryBox.Length + 5);
                        }
                        deliveryBox[deliveryBoxListSize] = new Produce(produceList[x]);
                        deliveryBoxListSize++;

                        totalCost += Int32.Parse(produceList[x].info()[1]) * Double.Parse(produceList[x].info()[3]);

                        removeVal(produceList, x, true);

                        x = prodListSize;
                    }
                }
                if (!found)
                {
                    break;
                }
            }
            if (Double.Parse(customer.info()[2]) < totalCost)
            {
                customer.changeBalance(-1 * Double.Parse(customer.info()[2]));
            }
            else
            {
                customer.changeBalance(-1 * totalCost);
            }
            totalCost = 0;
            int size = deliveryBoxListSize;
            for (int x = 0; x < size; size--)
            {
                customer.addProduce(deliveryBox[0]);
                removeVal(deliveryBox, 0, false);
            }
            deliveryBoxListSize = 0;
        }

        // Preconditions: None
        // Postconditions: array containg information about current items in delivery box are returned
        public string[] ShareOrder()
        {
            string[] output = new string[deliveryBoxListSize];
            string[] produceInfo;
            for (int i = 0; i < deliveryBoxListSize; i++)
            {
                produceInfo = deliveryBox[i].info();
                output[i] = (produceInfo[0] + " " + produceInfo[1] + " " + produceInfo[2]
                    + " " + produceInfo[3] + "$ " + produceInfo[4]);
            }
            return output;
        }

        // Preconditions: Optionally input vector of produce items if wanted to add them to storage
        // Postconditions: deliveryBox is filled by items dorm produceList based on validity and names from ForcastDelivery
        public void FillBox(Produce[] items = null, string testTime = "")
        {
            items = items ?? new Produce[0];
            for (int i = 0; i < items.Length; i++)
            {
                if (prodListSize == produceList.Length)
                {
                    Array.Resize(ref produceList, produceList.Length + 5);
                }
                produceList[prodListSize] = new Produce(items[i]);
                prodListSize++;
            }
            for (int i = 0; i < wantedItemsSize; i++)
            {
                for (int x = 0; x < prodListSize; x++)
                {
                    if (wantedItems[i] == produceList[x].info()[0] && totalCost <= Double.Parse(customer.info()[3]) && checkValid(produceList[x], testTime))
                    {
                        totalCost += Int32.Parse(produceList[x].info()[1]) * Double.Parse(produceList[x].info()[3]);

                        if (deliveryBoxListSize == deliveryBox.Length)
                        {
                            Array.Resize(ref deliveryBox, deliveryBox.Length + 5);
                        }
                        deliveryBox[deliveryBoxListSize] = new Produce(produceList[x]);
                        deliveryBoxListSize++;
                        removeVal(produceList, x, true);
                        x--;
                    }
                }
            }
        }

        // Preconditions: vector of desired produce item names must be inputed
        // Postconditions: fills wantedItems with inputed names.
        public void ForcastDelivery(string[] produceNames)
        {
            if (wantedItemsSize == 0)
            {
                wantedItems = new string[produceNames.Length];
            }
            for (int i = 0; i < (int)produceNames.Length; i++)
            {
                if (wantedItemsSize == wantedItems.Length)
                {
                    Array.Resize(ref wantedItems, wantedItems.Length + 5);
                }
                wantedItems[i] = (produceNames[i]);
                wantedItemsSize++;
            }
        }

        // Preconditions: produce array, index of item wanting to be removed, and bool of if its the produceList array must be inputed
        // Postconditions: removes the value at the given index of the given array and reduces respective size variable by 1
        private void removeVal(Produce[] arr, int index, bool isprodList)
        {
            int size;
            if (isprodList)
            {
                size = prodListSize;
            }
            else
            {
                size = deliveryBoxListSize;
            }

            Produce[] temp = new Produce[arr.Length];
            int newIndex = 0;
            for (int i = 0; i < size; i++)
            {
                if (i == index)
                {
                }
                else
                {
                    temp[newIndex] = new Produce(arr[i]);
                    newIndex++;
                }
            }

            if (isprodList)
            {
                prodListSize--;
                produceList = temp;
            }
            else
            {
                deliveryBoxListSize--;
                deliveryBox = temp;
            }
        }

    }
}

/*Implementation Invariants
  Public Functions:
  Replace Item
  -works by searching the deliveryBox for a produce item that shares the name and then replacing it wiht the inputed prodduce item
  -Uses check valid to make sure produce item is ok to be inputed
  DeliverBox
  -delegates work out to helper functions of segDeliverBox, pickyDeliverBox, and normDeliverBox based on the type of the customer object
  normDeliverBox
  -first checks if all items in the delivery box are valid, and replaces them with a random valid item in storage if it is nor valid.
  -if the minimum price has not been passed after this, then random valid items in storage are added until minimum price has been passed
  -then transfers all the items in delivery box to the customer and subtracts the total price from the customers balance, then sets totalprice to 0
  segDeliveryBox
  -works the same way as normDeliveryBox, but also checks if produce are of a valid classification whenever checking or adding produce in or to delivery box
  pickyDeliveryBox
  -works the same way as normDeliveryBox, but also checks if produce are not a forbidden item whenever checking or adding produce in or to delivery box
  ShareOrder
  -simply parses through the deliveryBox vector to return a string vector containing all the information about each produce item in there
  FillBox
  -Has an optional input in case the client wants to increase the storage, Fills the outgoing deliveryBox with the produce items that were requested in Forcast Delivery
  -will basically do nothing to deliveryBox if client hasnt called Forcast delivery yet (this is intentional)
  ForcastDelivery
  -simply uses a for loop to fill wanted items with all the the things in the inputed vector
  removeVal
  -helper function that removes value at a specific index of either the deliveryBox array or producelist array.
 */
