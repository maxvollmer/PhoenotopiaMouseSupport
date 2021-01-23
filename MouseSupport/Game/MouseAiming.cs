using UnityEngine;
using MouseSupport.Helpers;

namespace MouseSupport.Game
{
    public class MouseAiming
    {
        public static void UpdateSlingshotAim(GaleLogicOne galeLogicOne)
        {
            var sling_angle = CalcAimAngle();

            var localScale = galeLogicOne.transform.localScale;
            if (sling_angle > 90f && sling_angle < 270f)
            {
                sling_angle = 180f - sling_angle;
                localScale.x = -1f;
            }
            else
            {
                if (sling_angle > 180f)
                    sling_angle -= 360f;
                localScale.x = 1f;
            }
            galeLogicOne.transform.localScale = localScale;

            ReflectionHelper.SetMemberValue(galeLogicOne, "_sling_angle",
                Mathf.Clamp(sling_angle,
                ReflectionHelper.GetMemberValue<float>(galeLogicOne, "_sling_lower_limit"),
                ReflectionHelper.GetMemberValue<float>(galeLogicOne, "_sling_upper_limit"))
            );
        }

        /*
        public static void UpdateCrossbowAim(GaleLogicOne galeLogicOne)
        {
            ReflectionHelper.SetMemberValue(galeLogicOne, "velocity", ReflectionHelper.GetMemberValue<Vector3>(galeLogicOne, "velocity") * 0.85f);

            var crossbolt_angle = CalcAimAngle();

            if (crossbolt_angle > 90f && crossbolt_angle < 270f)
            {
                ReflectionHelper.SetMemberValue(galeLogicOne, "_cross_locked_left", true);
                ReflectionHelper.SetMemberValue(galeLogicOne, "_cross_locked_right", false);
            }
            else
            {
                ReflectionHelper.SetMemberValue(galeLogicOne, "_cross_locked_left", false);
                ReflectionHelper.SetMemberValue(galeLogicOne, "_cross_locked_right", true);
            }

            ReflectionHelper.SetMemberValue(galeLogicOne, "_crossbolt_angle", crossbolt_angle);

            var angleInRad = Mathf.Deg2Rad * crossbolt_angle;
            var reticule_distance = ReflectionHelper.GetMemberValue<float>(galeLogicOne, "_reticule_distance");
            var true_reticule_pos = new Vector3(reticule_distance * Mathf.Cos(angleInRad), reticule_distance * Mathf.Sin(angleInRad), 0f);

            ReflectionHelper.SetMemberValue(galeLogicOne, "_true_reticule_pos", true_reticule_pos);
            GL.C_ElasticMoveTransform(ReflectionHelper.GetMemberValue<Transform>(galeLogicOne, "_aim_reticule_trans"), true_reticule_pos, 0.75f, true);

            UpdateCrossbowAnimation(galeLogicOne, crossbolt_angle);
        }
        */

        private static float CalcAimAngle()
        {
            var cursorPos = MouseCursor.GetGameWorldCursorPos(out bool isNewCursorPos);

            var delta = cursorPos - (Vector2)PT2.gale_interacter.gale_sprite.transform.position;
            delta.Normalize();

            var angle = Mathf.Atan2(delta.x, delta.y);

            return NormalizeAngle(-(Mathf.Rad2Deg * angle - 90f));
        }

        private static float NormalizeAngle(float angle)
        {
            while (angle > 360f) angle -= 360f;
            while (angle < 0f) angle += 360f;
            return angle;
        }

        private static void UpdateCrossbowAnimation(GaleLogicOne galeLogicOne, float crossbolt_angle)
        {
            var anim = ReflectionHelper.GetMemberValue<Animator>(galeLogicOne, "_anim");
            int quadrant = GL.M_AngleToQuadrant(crossbolt_angle, 8);
            if (quadrant == 2)
            {
                if (crossbolt_angle > 90f)
                {
                    anim.SetInteger(GL.anim, 81);
                }
                else
                {
                    anim.SetInteger(GL.anim, 75);
                }
            }
            else if (quadrant == 6)
            {
                if (crossbolt_angle > 270f)
                {
                    anim.SetInteger(GL.anim, 79);
                }
                else
                {
                    anim.SetInteger(GL.anim, 82);
                }
            }
            else
            {
                anim.SetInteger(GL.anim, 73 + quadrant);
            }
        }
    }
}
