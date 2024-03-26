using System.Collections.Generic;

namespace API.KM58.Utility
{
    public class ResponseHelper
    {
        private static readonly List<ResponseMessage> StatusCodeArray = new List<ResponseMessage>
        {
            new ResponseMessage(400, false, "Thao tác thất bại!", "Yêu cầu không hợp lệ, vui lòng kiểm tra và thử lại."),
            new ResponseMessage(500, false, "Thao tác thất bại!", "Hiện tại do lượng khách hàng truy cập đăng ký khuyến mãi rất lớn, nên thao tác đăng ký nhận của bạn đã thất bại, vui lòng đăng ký lại sau ít phút. Xin cảm ơn!"),
            new ResponseMessage(404, false, "Thao tác thất bại!", "Không tìm thấy tài nguyên."),
            new ResponseMessage(4003, false, "Thao tác thất bại!", "Tài khoản của bạn không đủ điều kiện để tham gia khuyến mãi này. Để biết thêm chi tiết vui lòng liên hệ hỗ trợ 24/7 để được giải đáp."),
            new ResponseMessage(9000, false, "Thao tác thất bại!", "Quý khách nạp nhiều hơn số lần quy định, chưa thể tham gia khuyến mãi này."),
            new ResponseMessage(9001, false, "Thao tác thất bại!", "Tài khoản chưa phát sinh giao dịch hoặc đơn nạp tiền của bạn chưa thành công, chưa đủ điều kiện tham gia khuyến mãi này. Quý khách vui lòng thử lại hoặc liên hệ hỗ trợ 24/7 để được giải đáp!"),
            new ResponseMessage(9002, false, "Thao tác thất bại!", "Quý khách chưa nạp đủ điểm nạp tối thiểu để nhận khuyến mãi này."),
            new ResponseMessage(9003, false, "Thao tác thất bại!", "Quý khách chưa nạp đủ điểm cược hợp lệ tối thiểu để nhận khuyến mãi này."),
            new ResponseMessage(9004, false, "Thao tác thất bại!", "Bạn đã nhận khuyến mãi này, vui lòng kiểm tra lại hoặc đăng ký khuyến mãi khác."),
            new ResponseMessage(9005, false, "Thao tác thất bại!", "Thông tin đăng ký đã được gửi tới bộ phận liên quan xét duyệt. Kết quả sẽ được phản hồi vào hòm thư tin nhắn trên hệ thống. Quý khách vui lòng kiểm tra để biết thêm thông tin chi tiết."),
            new ResponseMessage(9006, false, "Thao tác thất bại!", "Quý khách chưa đủ điều kiện để nhận khuyến mãi. Kết quả đã được phản hồi vào hòm thư tin nhắn trên hệ thống. Quý khách vui lòng kiểm tra lại hoặc liên hệ CSKH 24/7 để biết thêm thông tin chi tiết."),
            new ResponseMessage(9007, false, "Thao tác liên tiếp!", "Quý khách vui lòng đợi sau 2 phút để tiếp tục đăng ký khuyến mãi. Xin cảm ơn."),
            new ResponseMessage(9008, false, "Thao tác thất bại!", "Quý khách đã nhận khuyến mãi song hành với khuyến mãi này, xin vui lòng chọn khuyến mãi khác hoặc liên hệ CSKH 24/7 để biết thêm chi tiết."),
            new ResponseMessage(9009, false, "Thao tác thất bại!", "Quý khách chưa có lịch sử cược, vui lòng kiểm tra và thử lại, hoặc liên hệ CSKH 24/7 để biết thêm chi tiết."),
            new ResponseMessage(9010, false, "Thao tác thất bại!", "Quá thời hạn đăng ký khuyến mãi, quý khách vui lòng kiểm tra lại hoặc liên hệ CSKH 24/7 để biết thêm chi tiết"),
            new ResponseMessage(9011, false, "Thao tác thất bại!", "Doanh thu cược của quý khách chưa đủ để đăng ký khuyến mãi này, vui lòng kiểm tra và thử lại."),
            new ResponseMessage(9012, false, "Thao tác thất bại!", "Quý khách đã tham gia cược, không thể tham gia khuyến mãi này."),
            new ResponseMessage(9013, false, "Thao tác thất bại!", "Quý khách đã có giao dịch rút tiền, không thể tham gia khuyến mãi này."),
            new ResponseMessage(9014, false, "Thao tác thất bại!", "Quý khách có lợi nhuận âm không đủ tối thiểu để tham gia khuyến mãi này."),
            new ResponseMessage(9015, false, "Thao tác thất bại!", "Quý khách không có lợi nhuận âm, không thể tham gia khuyến mãi này."),
            new ResponseMessage(9016, false, "Thao tác thất bại!", "Quá thời gian quy định kể từ lần nạp đầu tiên, Quý khách không thể tham gia khuyến mãi này."),
            new ResponseMessage(9017, false, "Thao tác thất bại!", "Quý khách nạp chưa đủ số lần quy định, chưa thể tham gia khuyến mãi này."),
            new ResponseMessage(9018, false, "Thao tác thất bại!", "Quý khách không có vé cược đủ điều kiện tham gia khuyến mãi này."),
            new ResponseMessage(9019, false, "Thao tác thất bại!", "Tài khoản của quý khách hiện tại không đủ điều kiện nhẫn khuyến mãi, vì thời gian nạp lần đầu đến lần nạp thứ 2 vượt quá thời gian quy định. Để biết thêm chi tiết vui lòng liên hệ CSKH 24/7"),
            new ResponseMessage(9020, false, "Thao tác thất bại!", "Quý khách cần hoàn thành doanh thu 3 vòng cược của tiền nạp lần 2 để nhận khuyến mãi này, xin cảm ơn"),
            new ResponseMessage(9021, false, "Thao tác thất bại!", "Quý khách không có vé cược hợp lệ trong ngày hôm nay"),
            new ResponseMessage(9023, false, "Thao tác thất bại!", "Những trò chơi hiển thị trước mã số vé cược sẽ không được tính là hợp lệ (Boxing King , Quyền Vương , Pháo thủ điên cuồng ... )"),
            new ResponseMessage(9024, false, "Thao tác thất bại!", "Quý khách không đủ điều kiện tham gia khuyến mãi này. (Cược hợp lệ x5 tiền thua, Tổng cược thua tối thiểu 2.500)"),
            new ResponseMessage(9025, false, "Thao tác thất bại!", "Số dư của quý khách không đủ để tham gia khuyến mãi này, vui lòng kiểm tra và thử lại"),
            new ResponseMessage(9026, false, "Thao tác thất bại!", "Mỗi thành viên chỉ được đăng ký nhận duy nhất 1 lần trong ngày! Quý khách vui lòng quay lại vào ngày hôm sau."),
            new ResponseMessage(9027, false, "Thao tác thất bại!", "Tổng nạp trong ngày của quý khác chưa đủ điểm nạp tối thiểu để nhận khuyến mãi này."),
            new ResponseMessage(9028, false, "Thao tác thất bại!", "Khuyến mãi không áp dụng trong những ngày vàng [2, 12, 22 & 15]. Quý khách vui lòng quay lại vào ngày hôm sau."),
            new ResponseMessage(9029, false, "Thao tác thất bại!", "Quý khách đã nhận khuyến mãi nạp tiền với lần nạp gần nhất này, xin vui lòng chọn khuyến mãi khác hoặc liên hệ CSKH 24/7 để biết thêm chi tiết."),
            new ResponseMessage(9030, false, "Thao tác thất bại!", "Khuyến mãi cần được đăng ký trước khi tham gia cược, quý khách vui lòng kiểm tra lại."),
            new ResponseMessage(9031, false, "Thao tác thất bại!", "Thành viên tham gia bằng nhiều tài khoản chỉ được hỗ trợ 01 tài khoản tham gia khuyến mãi/ngày.")
        };
    }

    public class ResponseMessage
    {
        public int StatusCode { get; }
        public bool Valid { get; }
        public string TitleMess { get; }
        public string TextMess { get; }

        public ResponseMessage(int statusCode, bool valid, string titleMess, string textMess)
        {
            StatusCode = statusCode;
            Valid = valid;
            TitleMess = titleMess;
            TextMess = textMess;
        }
    }
}
