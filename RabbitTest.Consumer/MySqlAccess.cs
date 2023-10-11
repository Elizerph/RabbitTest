using MySqlConnector;

namespace RabbitTest.Consumer
{
    public class MySqlAccess : IPersistent<InfoItem>
    {
        private bool _opened = false;
        private readonly MySqlConnection _connection;

        public MySqlAccess(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public async Task Save(InfoItem item)
        {
            if (!_opened)
            {
                await _connection.OpenAsync();
                _opened = true;
            }
            using var command = _connection.CreateCommand();
            command.CommandText = "insert into response (url, date, response) value (@url, @date, @response)";
            command.Parameters.AddWithValue("@url", item.Url);
            command.Parameters.AddWithValue("@date", item.Date);
            command.Parameters.AddWithValue("@response", item.Response);
            await command.ExecuteNonQueryAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _connection.DisposeAsync();
        }
    }
}
