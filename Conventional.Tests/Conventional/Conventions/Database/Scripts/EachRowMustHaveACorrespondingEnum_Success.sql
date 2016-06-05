CREATE TABLE dbo.CloudService 
(
    CloudServiceId INT NOT NULL,
    Code NVARCHAR(50) NOT NULL
)

INSERT INTO dbo.CloudService(CloudServiceId, Code)
VALUES (1, 'AWS')

INSERT INTO dbo.CloudService(CloudServiceId, Code)
VALUES (2, 'AZURE')
