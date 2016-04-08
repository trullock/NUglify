//@cc_on

//@set @fourteen = 14
//@set @foobar = (@fourteen * 02 - 0x08)

//@if ( @foobar == 20 )

  alert(/*@foobar@*/);
  alert(/*@notdefined @*/);
  
//@else
   
   alert("not twenty");

//@end



