
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;


/*

CREATE TABLE `tbl_auth_token` (
	`user_id` BIGINT(19) NOT NULL,
	`access_token` TEXT NULL DEFAULT NULL COLLATE 'utf8_bin',
	`access_token_expire_time` DATETIME NULL DEFAULT NULL,
	`access_token_update_time` DATETIME NULL DEFAULT NULL,
	`refresh_token` TEXT NULL DEFAULT NULL COLLATE 'utf8_bin',
	`refresh_token_expire_time` DATETIME NULL DEFAULT NULL,
	`refresh_token_update_time` DATETIME NULL DEFAULT NULL,
	PRIMARY KEY (`user_id`) USING BTREE
)
COLLATE='utf8_bin'
ENGINE=InnoDB
;
*/

public class AuthTokenEntity : Common.Entity
{
    public long user_id { get; internal set; }
    public string access_token { get; internal set; } = string.Empty;
    public DateTime access_token_expire_time { get; internal set; }
    public DateTime access_token_update_time { get; internal set; }
    public string refresh_token { get; internal set; } = string.Empty;
    public DateTime refresh_token_expire_time { get; internal set; }
    public DateTime refresh_token_update_time { get; internal set; }

    public static AuthTokenEntity Gen(
        long user_id,
        string accessToken, DateTime accessTokenExpire_time,
        string refreshToken, DateTime refreshTokenExpireTime
        )
    {
        return new AuthTokenEntity
        {
            user_id = user_id,
            access_token = accessToken,
            access_token_update_time = DateTime.Now,
            access_token_expire_time = accessTokenExpire_time,
            refresh_token = refreshToken,
            refresh_token_update_time = DateTime.Now,
            refresh_token_expire_time = refreshTokenExpireTime
        };
    }

    public AuthTokenEntity ChangeAccessToken(string accessToken, DateTime accessTokenExpireTime)
    {
        this.access_token = accessToken;
        this.access_token_update_time = DateTime.Now;
        this.access_token_expire_time = accessTokenExpireTime;
        return this;
    }

    public AuthTokenEntity ChangeRefreshToken(string refreshToken, DateTime refreshTokenExpireTime)
    {
        this.refresh_token = refreshToken;
        this.refresh_token_update_time = DateTime.Now;
        this.refresh_token_expire_time = refreshTokenExpireTime;
        return this;
    }

    public bool IsAccessTokenExpired()
    {
        return this.access_token_expire_time <= DateTime.Now;
    }
    public bool IsRefreshTokenExpired()
    {
        return this.refresh_token_expire_time <= DateTime.Now;
    }
}
