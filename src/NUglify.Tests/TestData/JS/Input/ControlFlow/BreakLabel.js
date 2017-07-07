outer_block: {
  console.log('1');
  break outer_block; // breaks out of both inner_block and outer_block
  console.log(':-('); // skipped
}
