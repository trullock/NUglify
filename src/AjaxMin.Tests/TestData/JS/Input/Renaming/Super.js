function one($bar, $ack, $gag)
{
    var $super = "ralph" + $bar;
    return $super + $ack + $gag;
}

function two($super, one, two)
{
    return $super + one - two;
}

function three(one, two, $super)
{
    return one - two + $super;
}
