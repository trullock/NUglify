outer_block: {
  console.log('1');
  break inner_block; // no such label
  console.log(':-('); // skipped
}
