-- public."VLeaderboards" source

CREATE OR REPLACE VIEW public."VLeaderboards"
AS SELECT (u."Username" || '#'::text) || u."Discriminator" AS "Username",
    l."UserId",
    l."Type",
    l."TimeType",
    l."Date",
    l."Score"
   FROM "Leaderboards" l
     JOIN "DiscordUsers" u ON l."UserId" = u."UserId";