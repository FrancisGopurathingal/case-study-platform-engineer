SELECT Operand1, Operand2, Type, Result, Timestamp
FROM Operations
ORDER BY Id DESC
LIMIT $limit;