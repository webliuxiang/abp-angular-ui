export const queryRulesArrOneOf = [
  { key: '是其中之一(is one of)', value: 'is one of', negate: false, ruleType: 'phrases' },
  { key: '不是其中任一(is not one of)', value: 'is not one of', negate: true, ruleType: 'phrases' },
];

export const queryRulesArrBetween = [
  { key: '在范围内(is between)', value: 'is between', negate: false, ruleType: 'range' },
  { key: '不在范围内(is not between)', value: 'is not between', negate: true, ruleType: 'range' },
];

export const queryRulesArrBase = [
  { key: '符合(term)', value: 'is', negate: false, ruleType: 'phrase' },
  { key: '不符合(term)', value: 'is not', negate: true, ruleType: 'term' },
  { key: '存在(exists)', value: 'exists', negate: false, ruleType: 'exists' },
  { key: '不存在(does not exist)', value: 'does not exist', negate: true, ruleType: 'exists' },
];

export const queryRules = [...queryRulesArrOneOf, ...queryRulesArrBetween, ...queryRulesArrBase];
