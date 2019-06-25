select reseh.CODE                               'MEETING_ROOM_ID',
       ord.naam                                 'MEETING_TITLE',
       reseh.NAAM                               'MEETING_DESCRIPTION',
       ord.AANTALPERS                           'MEETING_CAPACITY',
       pers.NAAM + ' ' + pers.VOORNAAM          'MEETING_ACTOR_ID',
       CONVERT(VARCHAR(33), ord.DATBEGIN, 126)  'MEETING_START',
       CONVERT(VARCHAR(33), ord.DATEINDE, 126)  'MEETING_END',
       CONVERT(VARCHAR(33), ord.DATINGAVE, 126) 'MEETING_CREATED',
       null                                     'MEETING_MODIFIED',
       null,
       null
from dbo.ord ord
         INNER JOIN dbo.RESEENH reseh
                    on ord.SYSRESEENH = reseh.SYSCODE
         INNER JOIN dbo.OBJALG obj
                    on reseh.SYSOBJALG = obj.SYSCODE
         INNER JOIN dbo.PERS pers
                    on ord.SYSMELDER = pers.SYSCODE
where
  --ord.CODE = '18033808.00' and
    obj.SYSCODE = '971'
  and obj.IS_ARCHIVED = 'F'
  and obj.naam not like '%TEST%'
  and year(datbegin) = year(getdate())
  and month(datbegin) = month(getdate())
  and day(datbegin) = day(getdate())
order by reseh.Code, ord.DATBEGIN