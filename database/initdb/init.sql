-- Create Employee table
CREATE TABLE IF NOT EXISTS Employee (
    Id UUID PRIMARY KEY,
    FirstName VARCHAR(255) NOT NULL,
    LastName VARCHAR(255) NOT NULL,
    Email VARCHAR(255) UNIQUE NOT NULL,
    CreatedAt TIMESTAMPTZ DEFAULT NOW()
);

-- Create LeaveRequestStatus enum type
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'leaverequeststatus') THEN
        CREATE TYPE LeaveRequestStatus AS ENUM ('Pending', 'Approved', 'Rejected', 'Cancelled');
    END IF;
END$$;

-- Create LeaveType enum type
DO $$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_type WHERE typname = 'leavetype') THEN
        CREATE TYPE LeaveType AS ENUM ('PaidLeave', 'SickLeave', 'MaternityLeave','UnpaidLeave','ExceptionalLeave');
    END IF;
END$$;

-- Create LeavePeriod type as a composite (start and end dates)
CREATE TABLE IF NOT EXISTS LeaveRequest (
    Id UUID PRIMARY KEY,
    EmployeeId UUID NOT NULL REFERENCES Employee(Id) ON DELETE CASCADE,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Type LeaveType NOT NULL,
    Comment TEXT,
    ManagerComment TEXT NULL,
    Status LeaveRequestStatus NOT NULL DEFAULT 'Pending',
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    ModifiedAt TIMESTAMPTZ NULL,
    CHECK (EndDate >= StartDate)
);

-- Insert sample employees
INSERT INTO Employee (Id, FirstName, LastName, Email)
VALUES
    ('11111111-1111-1111-1111-111111111111', 'Karine',    'Ferri',   'karine.ferri@example.com'),
    ('22222222-2222-2222-2222-222222222222', 'Jean-Pierre',    'Foucault', 'jeanpierre.foucault@example.com'),
    ('33333333-3333-3333-3333-333333333333', 'Laurent', 'Mariotte', 'laurent.mariotte@example.com'),
    ('44444444-4444-4444-4444-444444444444', 'Jean-Luc', 'Reichmann', 'jeanluc.reichmann@example.com');
; 

-- Insert a test leave request
INSERT INTO LeaveRequest (
    Id,
    EmployeeId,
    StartDate,
    EndDate,
    Type,
    Comment,
    ManagerComment,
    Status,
    CreatedAt,
    ModifiedAt
)
VALUES (
    '22222222-2222-2222-2222-222222222222',  -- test LeaveRequest Id
    '11111111-1111-1111-1111-111111111111',  -- existing Employee Id
    '2025-06-23',
    '2025-06-27',
    'PaidLeave', 
    'Test leave request for initial data',
    NULL,
    'Pending', 
    NOW(),
    NULL
);


