
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

/*

CREATE TABLE `tbl_user` (
	`id` BIGINT(19) NOT NULL,
	`email` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8_bin',
	`password` VARCHAR(1024) NULL DEFAULT NULL COLLATE 'utf8_bin',
	`name` VARCHAR(256) NULL DEFAULT NULL COLLATE 'utf8_bin',
	`user_role` VARCHAR(50) NULL DEFAULT NULL COLLATE 'utf8_bin',
	PRIMARY KEY (`id`) USING BTREE
)
COLLATE='utf8_bin'
ENGINE=InnoDB
;

*/

public class UserEntity : Common.Entity
{
    public long id { get; internal set; }
    public string email { get; internal set; } = string.Empty;
    public string password { get; internal set; } = string.Empty;
    public string name { get; internal set; } = string.Empty;
    public string userRole { get; internal set; } = string.Empty;

    public static UserEntity Gen(long id, string email, string password, string name)
    {
        return new UserEntity {
            id = id,
            email = email,
            password = password,
            name = name,
            userRole = "USER"
        };
    }

    public UserEntity ChangeEmail(string email)
    {
        this.email = email;
        return this;
    }

    public UserEntity ChangePassword(string password)
    {
        this.password = password;
        return this;
    }

    public UserEntity ChangeName(string name) 
    {  
        this.name = name;
        return this;
    }

    public UserEntity ChangeRole(string role)
    {
        this.userRole = role;
        return this;
    }
}
