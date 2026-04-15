CREATE TABLE IF NOT EXISTS Operations (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Operand1 REAL,
    Operand2 REAL,
    Type TEXT,
    Result REAL,
    Timestamp TEXT
);