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
    Status LeaveRequestStatus NOT NULL DEFAULT 'Pending',
    CreatedAt TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    ModifiedAt TIMESTAMPTZ NULL,
    CHECK (EndDate >= StartDate)
);

-- Insert sample employees
INSERT INTO Employee (Id, FirstName, LastName, Email) VALUES
('11111111-1111-1111-1111-111111111111', 'John', 'Doe', 'john.doe@example.com'),
('22222222-2222-2222-2222-222222222222', 'Jane', 'Smith', 'jane.smith@example.com'),
('33333333-3333-3333-3333-333333333333', 'Charlie', 'Brown', 'charlie.brown@example.com');
