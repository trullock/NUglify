function CreateRadixTable()
{
   var s, s1, s2, s3, x;                    //Declare variables.
   s = "Hex    Dec   Bin \n";               //Create table heading.
   for (x = 0; x < 16; x++)                 //Establish size of table
   {                                        // in terms of number of
      switch(x)                             // values shown.
      {                                     //Set intercolumn spacing.
         case 0 : 
            s1 = "      ";
            s2 = "    ";
            s3 = "   ";
            break;
         case 1 :
            s1 = "      ";
            s2 = "    ";
            s3 = "   ";
            break;
         case 2 :
            s3 = "  ";
            break;
         case 3 : 
            s3 = "  ";
            break;
         case 4 : 
            s3 = " ";
            break;
         case 5 :
            s3 = " ";
            break;
         case 6 : 
            s3 = " ";
            break;
         case 7 : 
            s3 = " ";
            break;
         case 8 :
            s3 = "" ;
            break;
         case 9 :
            s3 = "";
            break;
         default: 
            s1 = "     ";
            s2 = "";
            s3 = "    ";
      }                                     //Convert to hex, decimal & binary.
      s += " " + x.toString(16) + s1 + x.toString(10)
      s +=  s2 + s3 + x.toString(2)+ "\n";
   }
   return(s);                               //Return entire radix table.
}