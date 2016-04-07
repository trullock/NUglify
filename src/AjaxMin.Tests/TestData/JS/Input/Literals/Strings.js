(function(el,args)
{
  if ( !args ) { args = {}; }
  document.write('<script type="text/javascript">foo</script>');
  
  var toggleTextF = argWithDefault(args.togf,"°F"); // degree symbol might get escaped depending on output encoding
  var toggleTextC = argWithDefault(args.togc,"°C"); // degree symbol might get escaped depending on output encoding
  
  // this has multiple escapes separated by unescaped characters to make sure
  // that the string-escape logic properly mixes the two
  var twoEscapes = "how\tnow\tbrown\tcow";
  
  // multi-line string
  var multiLine = "Now is the time \
for all good men";

  var combined = "now is the time " + "for all good men";
  var scriptCombined = "</scri" + "pt>";

  var escapes = "\b\f\n\r\t\v\\";     // cross-browser differences in the way \v is handled means we should leave those string alone
  var doubleQuotes1 = "\"";           // should get switched to single-quote delimiter and no escape
  var doubleQuotes2 = '\"';           // escape should get removed
  var singleQuotes1 = "\'";           // escape should get removed
  var singleQuotes2 = '\'';           // should get switched to double-quote delimiter and no escape
  var copyrights = "©\xa9\u00a9\xA9\u00A9";  // depends on encoding sequence whether these will remain escaped
  var hexEsc = "\x39\xae\xAE";        // exercise numeric, lower, and upper digits for both digits
  var uniEsc = "\u328B\ubaaa\uFDF2";  // exercise numeric, lower, and upper digits for all four digits

  // these two characters (\u2028 and \u2029) should never be allowed unescaped in a string literal
  // because they will cause linebreaks in the string and throw script errors.
  // always make sure they are escaped, no matter what encoding we are using!
  var uniBreaks = "foo\u2028\u2029bar";
  
  function argWithDefault( arg, def )
  {
    return (typeof arg != "undefined" ? arg : def)
  }
}).as("Msn.HP.Weather");
