namespace WebApp.MVC.Models
{
    public class UsuarioRespostaLogin
    {        
        public string? AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken? UsuarioToken { get; set; }
        public ResponseResult ResponseResult { get; set; }

        public UsuarioRespostaLogin(ResponseResult? responseResult)
        {
            ResponseResult = responseResult ?? new ResponseResult();
        }

        public UsuarioRespostaLogin()
        {
            ResponseResult = new ResponseResult();
        }
    }
}