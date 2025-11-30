using Dapper;
using washit.models;

namespace washit.repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly DB _db;

        public ReservationRepository(DB db)
        {
            _db = db;
        }

        public async Task<Machine?> GetActiveMachineAsync(int washTypeId)
        {
            using var conn = _db.CreateConnection();
            try
            {

                string sql = @"
                SELECT TOP 1 * FROM Machines m
                WHERE m.WashTypeId = @washTypeId
                AND NOT EXISTS (
                    SELECT 1 FROM Reservations r
                    WHERE r.MachineId = m.Id AND r.IsActive = 1
                )";

                return await conn.QueryFirstOrDefaultAsync<Machine>(sql, new { washTypeId });
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<IEnumerable<Machine>> GetAllActiveMachineAsync()
        {
            using var conn = _db.CreateConnection();
            try
            {

                string sql = @"
                SELECT Id, MachineName, WashTypeId, IsActive FROM Machines m
                WHERE m.IsActive = 1";

                return await conn.QueryAsync<Machine>(sql);
            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<int> CreateReservationAsync(Reservation reservation)
        {
            using var conn = _db.CreateConnection();
            try
            {
                string sql = @"
                INSERT INTO Reservations (MachineId, UserId, WashTypeId, ReservedAt, IsActive, CreatedBy)
                VALUES (@MachineId, @UserId, @WashTypeId, @ReservedAt, 1, @CreatedBy);
                SELECT SCOPE_IDENTITY();";

                return await conn.ExecuteScalarAsync<int>(sql, reservation);

            }
            catch (System.Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();

            }
        }

        public async Task<bool> CancelReservationAsync(int reservationId, int? userId)
        {
            using var conn = _db.CreateConnection();

            try
            {

                string sql = @"UPDATE Reservations SET IsActive = 0 WHERE Id = @reservationId AND UserId = @userId AND IsActive = 1";
                return await conn.ExecuteAsync(sql, new { reservationId, userId }) > 0;

            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();

            }
        }

        public async Task<int> AddToWaitingListAsync(WaitingListEntry entry)
        {
            using var conn = _db.CreateConnection();
            try
            {
                string sql = @"
                INSERT INTO WaitingList (WashTypeId,CreatedBy,UserId)
                VALUES (@WashTypeId, @CreatedBy, @UserId);
                SELECT SCOPE_IDENTITY();";

                return await conn.ExecuteScalarAsync<int>(sql, entry);

            }
            catch (System.Exception)
            {

                throw;
            }
            finally
            {
                conn.Close();
            }
        }

        public async Task<WaitingListEntry?> GetNextWaitingUserAsync(int washTypeId)
        {
            try
            {

                using var conn = _db.CreateConnection();
                string sql = @"
        SELECT TOP 1 * FROM WaitingList
        WHERE WashTypeId = @WashTypeId AND Notified = 0
        ORDER BY CreatedAt ASC";

                return await conn.QueryFirstOrDefaultAsync<WaitingListEntry>(sql, new { washTypeId });
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public async Task MarkUserAsNotifiedAsync(int id)
        {
            string sql = @"UPDATE WaitingList SET Notified = 1 WHERE Id = @Id";

            using var conn = _db.CreateConnection();
            await conn.ExecuteAsync(sql, new { id });
        }

        public async Task<Reservation?> GetReservationByIdAsync(int reservationId)
        {
            string sql = @"SELECT Id, WashTypeId, UserId, IsActive FROM Reservations WHERE Id = @Id";

            using var conn = _db.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Reservation>(sql, new { Id = reservationId });
        }

        public async Task<Reservation?> GetActiveReservationByMachineIdAsync(int machineId)
        {
            using var conn = _db.CreateConnection();
            string sql = @"
            SELECT
            Id, MachineId, UserId, IsActive
            FROM Reservations
            WHERE MachineId = @MachineId
            AND IsActive = 1";
            return await conn.QueryFirstOrDefaultAsync<Reservation>(sql, new { MachineId = machineId });
        }

        public async Task<Reservation?> GetUserActiveReservationAsync(int? userId)
        {
            using var conn = _db.CreateConnection();

            string sql = @"
                            SELECT TOP 1 *
                            FROM Reservations
                            WHERE UserId = @UserId
                            AND IsActive = 1
                            ORDER BY ReservedAt DESC;
                        ";

            return await conn.QueryFirstOrDefaultAsync<Reservation>(sql, new { UserId = userId });
        }

    }
}
