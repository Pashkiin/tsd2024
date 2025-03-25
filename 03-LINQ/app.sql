-- task 1a
-- top 3 highest prices in the last year
SELECT * 
FROM Prices
WHERE Date BETWEEN DATEADD(YEAR, -1, GETDATE()) AND GETDATE()
ORDER BY Price DESC
LIMIT 3;

-- top 3 lowest prices in the last year
SELECT * 
FROM Prices
WHERE Date BETWEEN DATEADD(YEAR, -1, GETDATE()) AND GETDATE()
ORDER BY Price ASC
LIMIT 3;