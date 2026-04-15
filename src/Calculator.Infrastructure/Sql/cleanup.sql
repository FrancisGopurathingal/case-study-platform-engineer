DELETE FROM Operations
WHERE Id NOT IN (
    SELECT Id FROM Operations
    ORDER BY Id DESC
    LIMIT $limit
);