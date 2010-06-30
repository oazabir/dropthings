delete from UserSetting
where CurrentPageId NOT IN (select ID from Page)
GO

ALTER TABLE [dbo].[UserSetting]  WITH CHECK ADD  CONSTRAINT [FK_UserSetting_Page] FOREIGN KEY([CurrentPageId])
REFERENCES [dbo].[Page] ([ID])
GO

ALTER TABLE [dbo].[UserSetting] CHECK CONSTRAINT [FK_UserSetting_Page]
GO
