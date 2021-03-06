<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
	xmlns:dc="http://purl.org/dc/elements/1.1/"
	xmlns:dcterms="http://purl.org/dc/terms/1.1"
	xmlns:oai_dc="http://www.openarchives.org/OAI/2.0/oai_dc/"
  xmlns:qdc="http://epubs.cclrc.ac.uk/xmlns/qdc/"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
	xsi:schemaLocation="http://www.openarchives.org/OAI/2.0/oai_dc/
		http://www.openarchives.org/OAI/2.0/oai_dc.xsd"
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns="http://www.loc.gov/MARC21/slim"  exclude-result-prefixes="dc dcterms oai_dc">

  <xsl:import href="MARC21slimUtils.xsl"/>
  <xsl:output method="xml" encoding="UTF-8" indent="yes"/>


  <xsl:template match="/">
    <collection xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:schemaLocation="http://www.loc.gov/MARC21/slim http://www.loc.gov/standards/marcxml/schema/MARC21slim.xsd" >
      <xsl:apply-templates />
    </collection>
  </xsl:template>

  <xsl:template name="OAI-PMH">
    <xsl:for-each select = "ListRecords/record/metadata/oai_dc:dc">
      <xsl:apply-templates  />
    </xsl:for-each>
    <xsl:for-each select = "GetRecord/record/metadata/oai_dc:dc">
      <xsl:apply-templates  />
    </xsl:for-each>
  </xsl:template>

  <xsl:template match="text()" />
  <xsl:template match="qdc:qualifieddc">
    <xsl:variable name="date4" select="dc:date[1]" />
    <record>
      <leader>00000nkm  22000000u 4500</leader>
      <controlfield tag="008">000000s<xsl:value-of select="$date4" />    ksunnn        s   iueng d</controlfield>

      <datafield tag="042" ind1=" " ind2=" ">
        <subfield code="a">dc</subfield>
      </datafield>

      <xsl:for-each select="dc:title[1]">
        <datafield tag="245" ind1="0" ind2="0">
          <subfield code="a">
            <xsl:value-of select="."/>
          </subfield>
          <subfield code="h">[electronic resource].</subfield>
        </datafield>
      </xsl:for-each>

      <xsl:for-each select="dc:title[position()>1]">
        <xsl:if test=".!=''">
          <datafield tag="246" ind1="3" ind2="3">
            <subfield code="a">
              <xsl:value-of select="."/>
            </subfield>
          </datafield>
        </xsl:if>
      </xsl:for-each>


      <xsl:if test="dc:publisher">
        <datafield tag="260" ind1=" " ind2=" ">
          <subfield code="b">
            <xsl:value-of select="dc:publisher"/>
          </subfield>
          <subfield code="c">c<xsl:value-of select="dc:date[1]"/></subfield>
          <subfield code="c">
            <xsl:value-of select="dc:date[2]"/>
          </subfield>
        </datafield>
      </xsl:if>
      
      <xsl:for-each select="dc:coverage">
            <datafield tag="500" ind1=" " ind2=" ">
              <subfield code="a">
                <xsl:value-of select="."/>
              </subfield>
            </datafield>
      </xsl:for-each>

      <datafield tag="500" ind1=" " ind2=" ">
        <subfield code="a">
          <xsl:value-of select="dc:format[1]"/>
        </subfield>
      </datafield>

      <datafield tag="520" ind1="8" ind2=" ">
        <subfield code="a">
          <xsl:value-of select="dc:description[1]"/>
        </subfield>
      </datafield>

      <datafield tag="520" ind1="8" ind2=" ">
        <subfield code="a">
          <xsl:value-of select="dc:description[2]"/>
        </subfield>
      </datafield>

      <xsl:for-each select="dc:rights">
        <datafield tag="540" ind1=" " ind2=" ">
          <subfield code="a">
            <xsl:value-of select="."/>
          </subfield>
        </datafield>
      </xsl:for-each>


      <xsl:for-each select="dc:language">
        <datafield tag="546" ind1=" " ind2=" ">
          <subfield code="a">
            <xsl:value-of select="."/>
          </subfield>
        </datafield>
      </xsl:for-each>

      <xsl:for-each select="dc:subject">
        <xsl:call-template name="subj_template">
          <xsl:with-param name="field" select="'653'" />
          <xsl:with-param name="ind1" select="' '" />
          <xsl:with-param name="ind2" select="' '" />
          <xsl:with-param name="string" select="." />
          <xsl:with-param name="delimiter" select="';'" />
        </xsl:call-template>
      </xsl:for-each>
      
      <xsl:for-each select="dc:type">
        <datafield tag="655" ind1="4" ind2=" ">
          <subfield code="a">
            <xsl:value-of select="."/>
          </subfield>
          <subfield code="2">local</subfield>
        </datafield>
      </xsl:for-each>


      <datafield tag="710" ind1="2" ind2=" ">
        <subfield code="a">Ira Clemens Photo Album Collection</subfield>
      </datafield>


      <xsl:for-each select="dc:contributer">
        <xsl:call-template name="persname_template">
          <xsl:with-param name="string" select="." />
          <xsl:with-param name="field" select="'720'" />
          <xsl:with-param name="ind1" select = "' '" />
          <xsl:with-param name="ind2" select = "' '" />
          <xsl:with-param name="type" select="'contributor'" />
        </xsl:call-template>
      </xsl:for-each>

      <xsl:for-each select="dc:creator">
        <xsl:call-template name="persname_template">
          <xsl:with-param name="string" select="." />
          <xsl:with-param name="field" select="'720'" />
          <xsl:with-param name="ind1" select = "' '" />
          <xsl:with-param name="ind2" select = "' '" />
          <xsl:with-param name="type" select="'contributor'" />
        </xsl:call-template>
      </xsl:for-each>
      
      <xsl:for-each select="dc:publisher">
        <datafield tag="786" ind1="0" ind2="8">
          <subfield code="n">
            <xsl:value-of select="."/>
          </subfield>
        </datafield>
      </xsl:for-each>

      <xsl:for-each select="dc:relation">
        <datafield tag="787" ind1="0" ind2=" ">
          <subfield code="n">
            <xsl:value-of select="."/>
          </subfield>
        </datafield>
      </xsl:for-each>

      <xsl:if test="dc:identifier">
        <datafield tag="856" ind1="4" ind2="1">
          <subfield code="u">
            <xsl:value-of select="dc:identifier[last()]" />
          </subfield>
          <subfield code="y">View Online.</subfield>
        </datafield>
      </xsl:if>
    </record>

  </xsl:template>

  <!--Subject template-->

  <xsl:template name="subj_template">
    <xsl:param name="field" />
    <xsl:param name="ind1" />
    <xsl:param name="ind2" />
    <xsl:param name="string" />
    <xsl:param name="delimiter" />


    <xsl:choose>
      <!-- IF A PAREN, STOP AT AN OPENING semicolon -->
      <xsl:when test="contains($string, $delimiter)!=0">
        <xsl:variable name="newstem" select="substring-after($string, $delimiter)" />
        <datafield>
          <xsl:attribute name="tag">
            <xsl:value-of select="$field" />
          </xsl:attribute>

          <xsl:attribute name="ind1">
            <xsl:value-of select="$ind1" />
          </xsl:attribute>

          <xsl:attribute name="ind2">
            <xsl:value-of select="$ind2" />
          </xsl:attribute>
          <subfield code="a">
            <xsl:value-of select="substring-before($string, $delimiter)" />
          </subfield>
        </datafield>
        <!--Need to do recursion-->
        <xsl:call-template name="subj_template">
          <xsl:with-param name="field" select="'653'" />
          <xsl:with-param name="ind1" select="' '" />
          <xsl:with-param name="ind2" select="' '" />
          <xsl:with-param name="string" select="$newstem" />
          <xsl:with-param name="delimiter" select="';'" />
        </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <datafield>
          <xsl:attribute name="tag">
            <xsl:value-of select="$field" />
          </xsl:attribute>

          <xsl:attribute name="ind1">
            <xsl:value-of select="$ind1" />
          </xsl:attribute>

          <xsl:attribute name="ind2">
            <xsl:value-of select="$ind2" />
          </xsl:attribute>
          <subfield code="a">
            <xsl:value-of select="$string" />
          </subfield>
        </datafield>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="persname_template">
    <xsl:param name="string" />
    <xsl:param name="field" />
    <xsl:param name="ind1" />
    <xsl:param name="ind2" />
    <xsl:param name="type" />
    <datafield>
      <xsl:attribute name="tag">
        <xsl:value-of select="$field" />
      </xsl:attribute>
      <xsl:attribute name="ind1">
        <xsl:value-of select="$ind1" />
      </xsl:attribute>
      <xsl:attribute name="ind2">
        <xsl:value-of select="$ind2" />
      </xsl:attribute>

      <!-- Sample input: Brightman, Samuel C. (Samuel Charles), 1911-1992 -->
      <!-- Sample output: $aBrightman, Samuel C. $q(Samuel Charles), $d1911-. -->
      <!-- will handle names with dashes e.g. Bourke-White, Margaret -->

      <!-- CAPTURE PRIMARY NAME BY LOOKING FOR A PAREN OR A DASH OR NEITHER -->
      <xsl:choose>
        <!-- IF A PAREN, STOP AT AN OPENING PAREN -->
        <xsl:when test="contains($string, '(')!=0">
          <subfield code="a">
            <xsl:value-of select="substring-before($string, '(')" />
          </subfield>
        </xsl:when>
        <!-- IF A DASH, CHECK IF IT'S A DATE OR PART OF THE NAME -->
        <xsl:when test="contains($string, '-')!=0">
          <xsl:variable name="name_1" select="substring-before($string, '-')" />
          <xsl:choose>
            <!-- IF IT'S A DATE REMOVE IT -->
            <xsl:when test="translate(substring($name_1, (string-length($name_1)), 1), '0123456789', '9999999999') = '9'">
              <xsl:variable name="name" select="substring($name_1, 1, (string-length($name_1)-6))" />
              <subfield code="a">
                <xsl:value-of select="$name" />
              </subfield>
            </xsl:when>
            <!-- IF IT'S NOT A DATE, CHECK WHETHER THERE IS A DATE LATER -->
            <xsl:otherwise>
              <xsl:variable name="remainder" select="substring-after($string, '-')" />
              <xsl:choose>
                <!-- IF THERE'S A DASH, ASSUME IT'S A DATE AND REMOVE IT -->
                <xsl:when test="contains($remainder, '-')!=0">
                  <xsl:variable name="tmp" select="substring-before($remainder, '-')" />
                  <xsl:variable name="name_2" select="substring($tmp, 1, (string-length($tmp)-6))" />
                  <subfield code="a">
                    <xsl:value-of select="$name_1" />-<xsl:value-of select="$name_2" />
                  </subfield>
                </xsl:when>
                <!-- IF THERE'S NO DASH IN THE REMAINDER, OUTPUT IT -->
                <xsl:otherwise>
                  <subfield code="a">
                    <xsl:value-of select="$string" />
                  </subfield>
                </xsl:otherwise>
              </xsl:choose>
            </xsl:otherwise>
          </xsl:choose>
        </xsl:when>
        <!-- NO DASHES, NO PARENS, JUST OUTPUT THE NAME -->
        <xsl:otherwise>
          <subfield code="a">
            <xsl:value-of select="$string" />
          </subfield>
        </xsl:otherwise>
      </xsl:choose>

      <!-- CAPTURE SECONDARY NAME IN PARENS FOR SUBFIELD Q -->
      <xsl:if test="contains($string, '(')!=0">
        <xsl:variable name="subq_tmp" select="substring-after($string, '(')" />
        <xsl:variable name="subq" select="substring-before($subq_tmp, ')')" />
        <subfield code="q">
          (<xsl:value-of select="$subq" />)
        </subfield>
      </xsl:if>

      <!-- CAPTURE DATE FOR SUBFIELD D, ASSUME DATE IS LAST ITEM IN FIELD -->
      <!-- Note: does not work if name has a dash in it -->
      <xsl:if test="contains($string, '-')!=0">
        <xsl:variable name="date_tmp" select="substring-before($string, '-')" />
        <xsl:variable name="remainder" select="substring-after($string, '-')" />
        <xsl:choose>
          <!-- CHECK SECOND HALF FOR ANOTHER DASH; IF PRESENT, ASSUME THAT IS DATE -->
          <xsl:when test="contains($remainder, '-')!=0">
            <xsl:variable name="tmp" select="substring-before($remainder, '-')" />
            <xsl:variable name="date_1" select="substring($remainder, (string-length($tmp)-3))" />
            <!-- CHECK WHETHER IT HAS A NUMBER BEFORE IT AND IF SO, OUTPUT IT AS DATE -->
            <xsl:if test="translate(substring($date_1, 1, 1), '0123456789', '9999999999') = '9'">
              <subfield code="d"><xsl:value-of select="normalize-space($date_1)" />.</subfield>
            </xsl:if>
          </xsl:when>
          <!-- OTHERWISE THIS IS THE ONLY DASH SO TAKE IT -->
          <xsl:otherwise>
            <xsl:variable name="date_2" select="substring($string, (string-length($date_tmp)-3))" />
            <!-- CHECK WHETHER IT HAS A NUMBER BEFORE IT AND IF SO, OUTPUT IT AS DATE -->
            <xsl:if test="translate(substring($date_2, 1, 1), '0123456789', '9999999999') = '9'">
              <subfield code="d"><xsl:value-of select="normalize-space($date_2)" />.</subfield>
            </xsl:if>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:if>
      <subfield code="e"><xsl:value-of select="$type" /></subfield>
    </datafield>
  </xsl:template>

</xsl:stylesheet>